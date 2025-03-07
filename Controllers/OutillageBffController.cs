using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.Outillage;
using LimsUtils.Api;
using System.Text.Json;
using System.Net;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("/api/outillages")]
    public class OutillageBffController : Controller
    {
        private readonly HttpClient _httpClient;
        // Remplacer par l'URL de votre API Outillage
        private readonly string _outillageServiceUrl = "http://localhost:5213/api/outillages";

        public OutillageBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupère le nombre total d'outillages
        [HttpGet]
        [Route("/api/outillage/total")]
        public async Task<ActionResult<ApiResponse>> GetTotalOutillages()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_outillageServiceUrl + "/total");
            if (apiResponse == null) return NotFound();

            return Ok(apiResponse);
        }

        // Récupère une liste paginée d'outillages
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOutillages(int position = 1, int pageSize = 10)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_outillageServiceUrl + $"?position={position}&pageSize={pageSize}");
            if (apiResponse == null) return NotFound();

            return Ok(apiResponse);
        }

        // Récupère un outillage par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOutillage(int id)
        {
            string requestUri = $"{_outillageServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Outillage non trouvé");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de l'outillage");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // Crée un nouvel outillage
        [HttpPost]
        public async Task<ActionResult> CreateOutillage(OutillageDto outillageDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_outillageServiceUrl, outillageDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                return Ok(apiResponse);
            }
            else
            {
                return BadRequest("Une erreur est survenue lors de la création de l'outillage.");
            }
        }

        // Met à jour un outillage existant
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOutillage(int id, OutillageDto outillageDto)
        {
            string requestUri = $"{_outillageServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, outillageDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                return Ok(apiResponse);
            }
            else
            {
                return BadRequest("Une erreur est survenue lors de la mise à jour de l'outillage.");
            }
        }

        // Supprime un outillage
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteOutillage(int id)
        {
            string requestUri = $"{_outillageServiceUrl}/{id}";
            var response = await _httpClient.DeleteAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                return Ok(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Outillage supprimé avec succès.",
                    StatusCode = 200
                });
            }
            else
            {
                return BadRequest("Une erreur est survenue lors de la suppression de l'outillage.");
            }
        }
    }
}

