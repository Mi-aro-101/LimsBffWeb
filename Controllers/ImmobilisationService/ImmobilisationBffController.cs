using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LimsUtils.Api;
using System.Net;
using LimsBffWeb.Models.Immobilisation;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.ImmobilisationService
{
    [ApiController]
    [Route("/api/immobilisation")]
    public class ImmobilisationBffController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _immobilisationServiceUrl = "http://localhost:5066/api/immobilisations"; // URL de l'API Immobilisation

        public ImmobilisationBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupère le nombre total d'immobilisations
        [HttpGet]
        [Route("/api/immobilisation/total")]
        public async Task<ActionResult<ApiResponse>> GetTotalImmobilisations()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_immobilisationServiceUrl + "/total");
            if (apiResponse == null) return NotFound();

            return Ok(apiResponse);
        }

        // Récupère une liste paginée d'immobilisations
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetImmobilisations(int position = 1, int pageSize = 10)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_immobilisationServiceUrl + $"?position={position}&pageSize={pageSize}");
            if (apiResponse == null) return NotFound();

            return Ok(apiResponse);
        }

        // Endpoint de recherche via le BFF
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse>> SearchImmobilisations(string searchTerm = "")
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(
                _immobilisationServiceUrl + $"/search?searchTerm={searchTerm}");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // Récupère une immobilisation par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetImmobilisation(int id)
        {
            string requestUri = $"{_immobilisationServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Immobilisation non trouvée");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de l'immobilisation");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // Crée une nouvelle immobilisation
        [HttpPost]
        public async Task<ActionResult> CreateImmobilisation(ImmobilisationDto immobilisationDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_immobilisationServiceUrl, immobilisationDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<ImmobilisationDto>();
                ImmobilisationDto immobilisation = (ImmobilisationDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Une erreur est survenue lors de la création de l'immobilisation.");
        }

        // Met à jour une immobilisation existante
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateImmobilisation(int id, ImmobilisationDto immobilisationDto)
        {
            string requestUri = $"{_immobilisationServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, immobilisationDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                return Ok(apiResponse);
            }
            else return BadRequest("Une erreur est survenue lors de la mise à jour de l'immobilisation.");
        }

        // Supprime une immobilisation
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteImmobilisation(int id)
        {
            string requestUri = $"{_immobilisationServiceUrl}/{id}";
            var response = await _httpClient.DeleteAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                return Ok(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Immobilisation supprimée avec succès.",
                    StatusCode = 200
                });
            }
            else return BadRequest("Une erreur est survenue lors de la suppression de l'immobilisation.");
        }
    }
}
