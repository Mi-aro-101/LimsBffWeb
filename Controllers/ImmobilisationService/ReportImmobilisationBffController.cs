using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using LimsUtils.Api;
using LimsBffWeb.Models.Immobilisation;

namespace LimsBffWeb.Controllers.ReportImmobilisationService
{
    [ApiController]
    [Route("/api/report-immobilisation")]
    public class ReportImmobilisationBffController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _reportServiceUrl = "http://localhost:5066/api/report-immobilisations";

        public ReportImmobilisationBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupère le nombre total de rapports d'immobilisation
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalReports()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_reportServiceUrl + "/total");
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        // Récupère une liste paginée de rapports d'immobilisation
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetReports(int position = 1, int pageSize = 10)
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(
                $"{_reportServiceUrl}?position={position}&pageSize={pageSize}");
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        // Récupère un rapport d'immobilisation par ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetReportById(int id)
        {
            string requestUri = $"{_reportServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Rapport non trouvé");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération du rapport");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // Crée un nouveau rapport d'immobilisation
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateReport([FromBody] ReportImmobilisationDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(_reportServiceUrl, dto);
            using var responseStream = await response.Content.ReadAsStreamAsync();

            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                return Ok(apiResponse);
            }

            return BadRequest("Une erreur est survenue lors de la création du rapport.");
        }
  
    }
}
