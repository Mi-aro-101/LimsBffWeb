using Microsoft.AspNetCore.Mvc;
using LimsUtils.Api;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LimsBffWeb.Models.Outillage; // Namespace supposé pour les DTOs du BFF

namespace LimsBffWeb.Controllers.OutillageService
{
    [ApiController]
    [Route("/api/entree-outillages")]
    public class EntreeOutillageBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _entreeOutillageServiceUrl = "http://localhost:5213/api/entree-outillages";

        public EntreeOutillageBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/entree-outillages/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalEntreeOutillages()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_entreeOutillageServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/entree-outillages?position=1&pageSize=5
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetEntreeOutillages(int position = 1, int pageSize = 5)
        {
            if (position < 1)
                position = 1;
            if (pageSize < 1)
                pageSize = 5;

            string requestUrl = $"{_entreeOutillageServiceUrl}?position={position}&pageSize={pageSize}";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/entree-outillages/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetEntreeOutillage(int id)
        {
            string requestUrl = $"{_entreeOutillageServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Entrée outillage non trouvée.");
            else if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de l'entrée outillage.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // POST api/entree-outillages
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateEntreeOutillage([FromBody] EntreeOutillageDto entreeOutillageDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_entreeOutillageServiceUrl, entreeOutillageDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Une erreur est survenue lors de la création de l'entrée outillage.");
        }
    }
}