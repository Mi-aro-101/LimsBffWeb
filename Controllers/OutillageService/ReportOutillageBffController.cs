using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.Outillage;
using LimsUtils.Api;
using System.Text.Json;
using System.Net;
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.OutillageService
{
    [ApiController]
    [Route("/api/report-outillages")]
    public class ReportOutillageBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _reportOutillageServiceUrl = "http://localhost:5213/api/report-outillage";

        public ReportOutillageBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalReportOutillages()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_reportOutillageServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetReportOutillages(int position = 1, int pageSize = 10)
        {
            if (position < 1)
                position = 1;
            if (pageSize < 1)
                pageSize = 10;

            string requestUrl = $"{_reportOutillageServiceUrl}?position={position}&pageSize={pageSize}";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetReportOutillage(int id)
        {
            string requestUrl = $"{_reportOutillageServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Report outillage non trouvé.");
            else if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération du report outillage.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateReportOutillage([FromBody] ReportOutillageDto reportOutillageDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_reportOutillageServiceUrl, reportOutillageDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Une erreur est survenue lors de la création du report outillage.");
        }
    }
}