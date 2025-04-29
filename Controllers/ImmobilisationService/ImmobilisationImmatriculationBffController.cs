using Microsoft.AspNetCore.Mvc;
using LimsUtils.Api;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LimsBffWeb.Models.Immobilisation;

namespace LimsBffWeb.Controllers.ImmobilisationService
{
    [ApiController]
    [Route("/api/immobilisation-immatriculations")]
    public class ImmobilisationImmatriculationBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _immobilisationImmatriculationServiceUrl = "http://localhost:5066/api/immobilisation-immatriculations";

        public ImmobilisationImmatriculationBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/immobilisation-immatriculations/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalImmobilisationImmatriculations()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_immobilisationImmatriculationServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/immobilisation-immatriculations?position=1&pageSize=5
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetImmobilisationImmatriculations(int position = 1, int pageSize = 5)
        {
            if (position < 1)
                position = 1;
            if (pageSize < 1)
                pageSize = 5;

            string requestUrl = $"{_immobilisationImmatriculationServiceUrl}?position={position}&pageSize={pageSize}";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/immobilisation-immatriculations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetImmobilisationImmatriculation(int id)
        {
            string requestUrl = $"{_immobilisationImmatriculationServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Immatriculation d'immobilisation non trouvée.");
            else if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de l'immatriculation d'immobilisation.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // POST api/immobilisation-immatriculations
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateImmobilisationImmatriculation([FromBody] ImmobilisationImmatriculationDto immobilisationImmatriculationDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_immobilisationImmatriculationServiceUrl, immobilisationImmatriculationDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Une erreur est survenue lors de la création de l'immatriculation d'immobilisation.");
        }
    }
}