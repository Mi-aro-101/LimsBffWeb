using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LimsUtils.Api;
using System.Net.Http.Json;
using System.Net;
using LimsBffWeb.Models.Immobilisation; // Assurez-vous que ce namespace contient IndisponibiliteDto
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.ImmobilisationService
{
    [ApiController]
    [Route("api/indisponibilites")]
    public class IndisponibiliteBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _indisponibiliteServiceUrl = "http://localhost:5066/api/indisponibilites"; // URL de l'API backend

        public IndisponibiliteBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupérer le total des indisponibilités
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalIndisponibilites()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_indisponibiliteServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la récupération du total",
                    StatusCode = 404
                });
            return Ok(apiResponse);
        }

        // Récupérer les indisponibilités avec pagination
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetIndisponibilites(int position = 1, int pageSize = 10)
        {
            string requestUri = $"{_indisponibiliteServiceUrl}?position={position}&pageSize={pageSize}";
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            if (apiResponse == null)
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la récupération des indisponibilités",
                    StatusCode = 404
                });
            return Ok(apiResponse);
        }

        // Récupérer une indisponibilité par ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetIndisponibilite(int id)
        {
            string requestUri = $"{_indisponibiliteServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Indisponibilité non trouvée",
                    StatusCode = 404
                });
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la récupération de l'indisponibilité",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // Créer une nouvelle indisponibilité
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateIndisponibilite([FromBody] IndisponibiliteDto indisponibiliteDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_indisponibiliteServiceUrl, indisponibiliteDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);

            if (response.IsSuccessStatusCode && apiResponse?.Data != null)
            {
                // Retourner un statut 201 avec les données, sans générer une URL via CreatedAtAction
                return StatusCode(201, apiResponse);
            }
            else
            {
                return BadRequest(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la création de l'indisponibilité",
                    StatusCode = (int)response.StatusCode
                });
            }
        }
        // Dans IndisponibiliteBffController.cs
[HttpGet("available-immobilisations")]
public async Task<ActionResult<ApiResponse>> GetAvailableImmobilisationsImmatriculees()
{
    string requestUri = $"{_indisponibiliteServiceUrl}/available-immobilisations";
    var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
    if (apiResponse == null)
        return NotFound(new ApiResponse
        {
            Data = null,
            ViewBag = null,
            IsSuccess = false,
            Message = "Erreur lors de la récupération des immobilisations disponibles",
            StatusCode = 404
        });
    return Ok(apiResponse);
}
    }
}