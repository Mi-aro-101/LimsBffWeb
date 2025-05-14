using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.ReactifService;
using System.Text.Json;
using LimsUtils.Api;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LimsBffWeb.Models;

namespace LimsBffWeb.Controllers.ReactifService
{
    [ApiController]
    [Route("/api/reactifs")]
    public class ReactifBffController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _reactifServiceUrl = "http://localhost:5073/api/reactifs"; // URL de l'API Reactif

        public ReactifBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupère le nombre total de réactifs
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalReactifs()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_reactifServiceUrl + "/total");
            if (apiResponse == null) return NotFound();

            return Ok(apiResponse);
        }

        // Récupère une liste paginée de réactifs
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetReactifs(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1;
            if (pageSize < 1) pageSize = 10;

            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_reactifServiceUrl + $"?position={position}&pageSize={pageSize}");
            if (apiResponse == null) return NotFound();

            return Ok(apiResponse);
        }

        // Endpoint de recherche via le BFF pour les réactifs
        [HttpGet("search")]
public async Task<ActionResult<ApiResponse>> SearchReactifs([FromQuery] string searchTerm = "")
{
    ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(
        _reactifServiceUrl + $"/search?searchTerm={Uri.EscapeDataString(searchTerm)}");
    if (apiResponse == null)
        return NotFound();

    return Ok(apiResponse);
}


        // Récupère un réactif par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetReactif(int id)
        {
            string requestUri = $"{_reactifServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Reactif non trouvé");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération du reactif");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // Crée un nouveau réactif
        [HttpPost]
        public async Task<ActionResult> CreateReactif([FromBody] ReactifDto reactifDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_reactifServiceUrl, reactifDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                return Ok(apiResponse);
            }
            else
            {
                return BadRequest("Une erreur est survenue lors de la création du reactif.");
            }
        }

        // Met à jour un réactif existant
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReactif(int id, [FromBody] ReactifDto reactifDto)
        {
            string requestUri = $"{_reactifServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, reactifDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                return Ok(apiResponse);
            }
            else
            {
                return BadRequest("Une erreur est survenue lors de la mise à jour du reactif.");
            }
        }

        // Supprime un réactif
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteReactif(int id)
        {
            string requestUri = $"{_reactifServiceUrl}/{id}";
            var response = await _httpClient.DeleteAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                return Ok(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Reactif supprimé avec succès.",
                    StatusCode = 200
                });
            }
            else
            {
                return BadRequest("Une erreur est survenue lors de la suppression du reactif.");
            }
        }

        [HttpPost("reste-stock")]
        public async Task<ActionResult<ApiResponse>> GetResteStock([FromBody] ResteStockDto resteStockDto)
        {
            string requestUri = _reactifServiceUrl + "/reste-stock";
            var response = await _httpClient.PostAsJsonAsync(requestUri, resteStockDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            return Ok(apiResponse);
        }
    }
}
