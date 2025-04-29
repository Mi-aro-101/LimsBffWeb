using Microsoft.AspNetCore.Mvc;
using LimsUtils.Api;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LimsBffWeb.Models.Immobilisation;

namespace LimsBffWeb.Controllers.ImmobilisationService
{
    [ApiController]
    [Route("/api/entree-immobilisations")]
    public class EntreeImmobilisationBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _entreeImmobilisationServiceUrl = "http://localhost:5066/api/entree-immobilisations";

        public EntreeImmobilisationBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/entree-immobilisations/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalEntreeImmobilisations()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_entreeImmobilisationServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/entree-immobilisations?position=1&pageSize=5
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetEntreeImmobilisations(int position = 1, int pageSize = 5)
        {
            if (position < 1)
                position = 1;
            if (pageSize < 1)
                pageSize = 5;

            string requestUrl = $"{_entreeImmobilisationServiceUrl}?position={position}&pageSize={pageSize}";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/entree-immobilisations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetEntreeImmobilisation(int id)
        {
            string requestUrl = $"{_entreeImmobilisationServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Entrée immobilisation non trouvée.");
            else if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de l'entrée immobilisation.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // POST api/entree-immobilisations
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateEntreeImmobilisation([FromBody] EntreeImmobilisationDto entreeImmobilisationDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_entreeImmobilisationServiceUrl, entreeImmobilisationDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Une erreur est survenue lors de la création de l'entrée immobilisation.");
        }

        // GET api/entree-immobilisations/non-immatriculees
        [HttpGet("non-immatriculees")]
        public async Task<ActionResult<ApiResponse>> GetEntreeImmobilisationsNonImmatriculees()
        {
            string requestUrl = $"{_entreeImmobilisationServiceUrl}/non-immatriculees";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }
    }
}