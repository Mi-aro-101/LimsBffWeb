using Microsoft.AspNetCore.Mvc;
using LimsUtils.Api;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LimsBffWeb.Models.ReactifService;

namespace LimsBffWeb.Controllers.ReactifService
{
    [ApiController]
    [Route("/api/report-reactifs")]
    public class ReportReactifBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _reportReactifServiceUrl = "http://localhost:5073/api/report-reactif"; // URL spécifiée

        public ReportReactifBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/report-reactifs/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalReportReactifs()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_reportReactifServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/report-reactifs?position=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetReportReactifs(int position = 1, int pageSize =10)
        {
            if (position < 1)
                position = 1;
            if (pageSize < 1)
                pageSize = 10;

            string requestUrl = $"{_reportReactifServiceUrl}?position={position}&pageSize={pageSize}";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/report-reactif/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetReportReactif(int id)
        {
            string requestUrl = $"{_reportReactifServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Rapport réactif non trouvé.");
            else if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération du rapport réactif.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // POST api/report-reactifs
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateReportReactif([FromBody] ReportReactifDto reportReactifDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_reportReactifServiceUrl, reportReactifDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Une erreur est survenue lors de la création du rapport réactif.");
        }
    }
}