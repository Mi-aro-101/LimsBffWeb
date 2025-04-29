using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LimsUtils.Api;
using System.Net.Http.Json;
using System.Net;
using LimsBffWeb.Models.Immobilisation;

namespace LimsBffWeb.Controllers.ImmobilisationService
{
    [ApiController]
    [Route("api/localisations")]
    public class LocalisationBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        // URL de l'API distante pour les localisations
        private readonly string _localisationServiceUrl = "http://localhost:5066/api/localisations";

        public LocalisationBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupérer le total des localisations
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalLocalisations()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_localisationServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();
            return Ok(apiResponse);
        }

        // Récupérer les localisations avec pagination
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetLocalisations(int position = 1, int pageSize = 10)
        {
            string requestUri = $"{_localisationServiceUrl}?position={position}&pageSize={pageSize}";
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            if (apiResponse == null)
                return NotFound();
            return Ok(apiResponse);
        }

        // Récupérer une localisation par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetLocalisation(int id)
        {
            string requestUri = $"{_localisationServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Localisation non trouvée");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de la localisation");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // Créer une localisation
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateLocalisation(LocalisationDto localisationDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_localisationServiceUrl, localisationDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Created("", apiResponse);
            else
                return BadRequest("Erreur lors de la création de la localisation.");
        }

        // Mettre à jour une localisation
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateLocalisation(int id, LocalisationDto localisationDto)
        {
            string requestUri = $"{_localisationServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, localisationDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Erreur lors de la mise à jour.");
        }

        // Supprimer une localisation
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteLocalisation(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_localisationServiceUrl}/{id}");
            if (response.IsSuccessStatusCode)
                return Ok(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Localisation supprimée avec succès.",
                    StatusCode = 200
                });
            else
                return BadRequest("Erreur lors de la suppression.");
        }
    }
}