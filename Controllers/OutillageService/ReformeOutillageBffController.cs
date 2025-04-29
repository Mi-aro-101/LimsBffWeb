using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.Outillage;
using LimsUtils.Api;
using System.Text.Json;
using System.Net;
using System.Threading.Tasks;


namespace LimsBffWeb.Controllers.OutillageService
{
    [ApiController]
    [Route("/api/reforme-outillages")]
    public class ReformeOutillageBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _reformeOutillageServiceUrl = "http://localhost:5213/api/reforme-outillage";

        public ReformeOutillageBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalReformeOutillages()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_reformeOutillageServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetReformeOutillages(int position = 1, int pageSize = 10)
        {
            if (position < 1)
                position = 1;
            if (pageSize < 1)
                pageSize = 10;

            string requestUrl = $"{_reformeOutillageServiceUrl}?position={position}&pageSize={pageSize}";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetReformeOutillage(int id)
        {
            string requestUrl = $"{_reformeOutillageServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Réforme outillage non trouvée.");
            else if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de la réforme outillage.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateReformeOutillage([FromBody] ReformeOutillageDto reformeOutillageDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_reformeOutillageServiceUrl, reformeOutillageDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Une erreur est survenue lors de la création de la réforme outillage.");
        }
    }
}