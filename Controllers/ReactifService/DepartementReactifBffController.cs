using Microsoft.AspNetCore.Mvc;
using LimsUtils.Api;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace LimsBffWeb.Controllers.ReactifService
{
    [ApiController]
    [Route("/api/departements-reactif")] // Route personnalisée pour éviter les conflits
    public class DepartementReactifBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _departementServiceUrl = "http://localhost:5073/api/departements"; // Nouvelle URL cible

        public DepartementReactifBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/departements-reactif/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalDepartements()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_departementServiceUrl}/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/departements-reactif?position=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetDepartements(int position = 1, int pageSize = 10)
        {
            // Validation des paramètres
            if (position < 1) position = 1;
            if (pageSize < 1) pageSize = 10;

            string requestUrl = $"{_departementServiceUrl}?position={position}&pageSize={pageSize}";
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);

            return apiResponse == null
                ? NotFound()
                : Ok(apiResponse);
        }

        // GET api/departements-reactif/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetDepartement(int id)
        {
            string requestUrl = $"{_departementServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Département non trouvé.");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération du département.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }
    }
}