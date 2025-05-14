using Microsoft.AspNetCore.Mvc;
using LimsUtils.Api;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LimsBffWeb.Models.ReactifService;
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.ReactifService
{
    [ApiController]
    [Route("/api/sortie-reactifs")]
    public class SortieReactifBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        // URL de l'API back pour les sorties réactifs
        private readonly string _sortieReactifServiceUrl = "http://localhost:5073/api/sortie-reactifs";

        public SortieReactifBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/sortie-reactifs/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalSortieReactifs()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_sortieReactifServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/sortie-reactifs?position=1&pageSize=5
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetSortieReactifs(int position = 1, int pageSize = 5)
        {
            if (position < 1)
                position = 1;
            if (pageSize < 1)
                pageSize = 5;

            string requestUrl = $"{_sortieReactifServiceUrl}?position={position}&pageSize={pageSize}";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/sortie-reactifs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetSortieReactif(int id)
        {
            string requestUrl = $"{_sortieReactifServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Sortie réactif non trouvée.");
            else if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de la sortie réactif.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // POST api/sortie-reactifs
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateSortieReactif([FromBody] SortieReactifDto sortieReactifDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_sortieReactifServiceUrl, sortieReactifDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            return Ok(apiResponse);
        }

        
    }
}
