using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.Outillage;
using LimsUtils.Api;
using System.Text.Json;
using System.Net;
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.OutillageService
{
    [ApiController]
    [Route("/api/outillages")]
    public class OutillageBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _outillageServiceUrl = "http://localhost:5213/api/outillages";

        public OutillageBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("/api/outillage/total")]
        public async Task<ActionResult<ApiResponse>> GetTotalOutillages()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_outillageServiceUrl + "/total");
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOutillages(int position = 1, int pageSize = 10)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_outillageServiceUrl + $"?position={position}&pageSize={pageSize}");
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

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

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse>> SearchOutillages(string searchTerm = "")
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(
                _outillageServiceUrl + $"/search?searchTerm={searchTerm}");
            if (apiResponse == null)
                return NotFound();
            return Ok(apiResponse);
        }
    }
}