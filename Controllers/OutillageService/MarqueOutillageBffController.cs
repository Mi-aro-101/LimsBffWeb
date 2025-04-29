using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.Outillage;
using LimsUtils.Api;
using System.Text.Json;
using System.Net;

namespace LimsBffWeb.Controllers.OutillageService
{
    [ApiController]
    [Route("/api/marque-outillage")]
    public class MarqueOutillageBffController : Controller
    {
        private readonly HttpClient _httpClient;
        // Remplacez par l'URL de votre API Marque dans le service Outillage
        private readonly string _marqueServiceUrl = "http://localhost:5213/api/marques";

        public MarqueOutillageBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupère le nombre total de marques
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalMarques()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_marqueServiceUrl + "/total");
            if (apiResponse == null)
            {
                return NotFound();
            }
            return Ok(apiResponse);
        }

        // Récupère une liste paginée de marques
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetMarques(int position = 1, int pageSize = 10)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(
                _marqueServiceUrl + $"?position={position}&pageSize={pageSize}"
            );
            if (apiResponse == null)
            {
                return NotFound();
            }
            return Ok(apiResponse);
        }

        // Récupère une marque par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMarque(int id)
        {
            string requestUri = $"{_marqueServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Marque non trouvée");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de la marque");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }
    }
}
