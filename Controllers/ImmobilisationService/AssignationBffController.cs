using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LimsUtils.Api;
using LimsBffWeb.Models.Immobilisation;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace LimsBffWeb.Controllers.AssignationService
{
    [ApiController]
    [Route("/api/assignation")]
    public class AssignationBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _assignationServiceUrl = "http://localhost:5066/api/assignations"; // URL de l'API Assignation

        public AssignationBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupère le nombre total d'assignations
        [HttpGet]
        [Route("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalAssignations()
        {
            var response = await _httpClient.GetAsync($"{_assignationServiceUrl}/total");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la récupération du total des assignations : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }

        // Récupère une liste paginée d'assignations
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAssignations(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1;
            if (pageSize < 1) pageSize = 10;

            var response = await _httpClient.GetAsync($"{_assignationServiceUrl}?position={position}&pageSize={pageSize}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la récupération des assignations : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }

        // Endpoint de recherche via le BFF
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse>> SearchAssignations(string searchTerm = "", int position = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync($"{_assignationServiceUrl}/search?searchTerm={Uri.EscapeDataString(searchTerm)}&position={position}&pageSize={pageSize}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la recherche des assignations : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }

        // Récupère une assignation par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetAssignation(int id)
        {
            var response = await _httpClient.GetAsync($"{_assignationServiceUrl}/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Assignation non trouvée",
                    StatusCode = 404
                });
            }
            else if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la récupération de l'assignation : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }

        // Crée une nouvelle assignation
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateAssignation([FromBody] AssignationDto assignationDto)
        {
            if (assignationDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Le corps de la requête ne peut pas être vide.",
                    StatusCode = 400
                });
            }

            // Forcer l'ID à null pour une création
            assignationDto.IdAssignation = null;

            // Configurer les options de sérialisation pour ignorer les propriétés imbriquées inutiles
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Sérialiser uniquement les champs nécessaires
            var simplifiedDto = new
            {
                assignationDto.DateAssignation,
                assignationDto.IdLocalisation,
                assignationDto.IdImmobilisationPropre,
                assignationDto.IdEmploye
            };

            var content = new StringContent(
                JsonSerializer.Serialize(simplifiedDto, jsonOptions),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(_assignationServiceUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erreur lors de la création de l'assignation : {response.StatusCode} - {errorContent}");
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la création de l'assignation : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (apiResponse?.Data == null)
            {
                return StatusCode(500, new ApiResponse
                {
                    IsSuccess = false,
                    Message = "La création a réussi mais aucune donnée n'a été renvoyée par l'API.",
                    StatusCode = 500
                });
            }

            // Désérialiser la réponse pour obtenir l'ID créé
            var createdAssignationJson = JsonSerializer.Serialize(apiResponse.Data);
            var createdAssignation = JsonSerializer.Deserialize<AssignationDto>(createdAssignationJson, jsonOptions);

            if (createdAssignation == null || createdAssignation.IdAssignation == null)
            {
                return StatusCode(500, new ApiResponse
                {
                    IsSuccess = false,
                    Message = "L'ID de l'assignation créée n'a pas été renvoyé correctement.",
                    StatusCode = 500
                });
            }

            return CreatedAtAction(nameof(GetAssignation), new { id = createdAssignation.IdAssignation.Value }, apiResponse);
        }

        // Met à jour une assignation existante
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateAssignation(int id, [FromBody] AssignationDto assignationDto)
        {
            if (assignationDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Le corps de la requête ne peut pas être vide.",
                    StatusCode = 400
                });
            }

            if (id != assignationDto.IdAssignation.GetValueOrDefault())
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "L'ID dans l'URL ne correspond pas à l'ID dans le corps de la requête.",
                    StatusCode = 400
                });
            }

            // Configurer les options de sérialisation pour ignorer les propriétés imbriquées inutiles
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Sérialiser uniquement les champs nécessaires pour la mise à jour
            var simplifiedDto = new
            {
                assignationDto.IdAssignation,
                assignationDto.DateAssignation,
                assignationDto.IdLocalisation,
                assignationDto.IdEmploye
                // Note : IdImmobilisationPropre n'est pas inclus car il ne doit pas être modifié lors de la mise à jour
            };

            var content = new StringContent(
                JsonSerializer.Serialize(simplifiedDto, jsonOptions),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"{_assignationServiceUrl}/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la mise à jour de l'assignation : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }

        // Récupère un employé par son matricule
        [HttpGet("employe/{matricule}")]
        public async Task<ActionResult<ApiResponse>> GetEmployeByMatricule(string matricule)
        {
            var response = await _httpClient.GetAsync($"{_assignationServiceUrl}/employe/{Uri.EscapeDataString(matricule)}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Employé non trouvé",
                    StatusCode = 404
                });
            }
            else if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la récupération de l'employé : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }

        // Récupère les immobilisations disponibles
        [HttpGet("available-immobilisations")]
        public async Task<ActionResult<ApiResponse>> GetAvailableImmobilisations()
        {
            var response = await _httpClient.GetAsync($"{_assignationServiceUrl}/available-immobilisations");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la récupération des immobilisations disponibles : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }

        // Récupère toutes les localisations
        [HttpGet("localisations")]
        public async Task<ActionResult<ApiResponse>> GetLocalisations()
        {
            var response = await _httpClient.GetAsync($"{_assignationServiceUrl}/localisations");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la récupération des localisations : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }

        [HttpGet("assigned-immobilisations")]
        public async Task<ActionResult<ApiResponse>> GetAssignedImmobilisations()
        {
            var response = await _httpClient.GetAsync($"{_assignationServiceUrl}/assigned-immobilisations");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la récupération des immobilisations assignées : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }

        [HttpGet("current/{idImmobilisationPropre}")]
        public async Task<ActionResult<ApiResponse>> GetCurrentAssignation(int idImmobilisationPropre)
        {
            var response = await _httpClient.GetAsync($"{_assignationServiceUrl}/current/{idImmobilisationPropre}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la récupération de l'assignation actuelle : {errorContent}",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse ?? new ApiResponse { IsSuccess = false, Message = "Réponse inattendue de l'API", StatusCode = 500 });
        }
    }
}