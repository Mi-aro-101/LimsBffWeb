using Microsoft.AspNetCore.Mvc;
using LimsUtils.Api;
using System.Net;
using System.Text.Json;
using LimsBffWeb.Models.ReactifService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.ReactifService
{
    [ApiController]
    [Route("/api/entree-reactifs")]
    public class EntreeReactifBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _entreeReactifServiceUrl = "http://localhost:5073/api/entree-reactifs";

        public EntreeReactifBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalEntreeReactifs()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_entreeReactifServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetEntreeReactifs(int position = 1, int pageSize = 5)
        {
            if (position < 1)
                position = 1;
            if (pageSize < 1)
                pageSize = 5;

            string requestUrl = $"{_entreeReactifServiceUrl}?position={position}&pageSize={pageSize}";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetEntreeReactif(int id)
        {
            string requestUrl = $"{_entreeReactifServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Entrée réactif non trouvée.");
            else if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de l'entrée réactif.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateEntreeReactif([FromBody] EntreeReactifDto entreeReactifDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_entreeReactifServiceUrl, entreeReactifDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Une erreur est survenue lors de la création de l'entrée réactif.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateEntreeReactif(int id, [FromBody] EntreeReactifDto entreeReactifDto)
        {
            string requestUrl = $"{_entreeReactifServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(requestUrl, entreeReactifDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Une erreur est survenue lors de la mise à jour de l'entrée réactif.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteEntreeReactif(int id)
        {
            string requestUrl = $"{_entreeReactifServiceUrl}/{id}";
            var response = await _httpClient.DeleteAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return Ok(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Entrée réactif supprimée avec succès.",
                    StatusCode = 200
                });
            }
            else
            {
                return BadRequest("Une erreur est survenue lors de la suppression de l'entrée réactif.");
            }
        }

        [HttpGet("depenses/mois/{annee}")]
        public async Task<ActionResult<ApiResponse>> GetDepensesParMois(int annee)
        {
            string requestUrl = $"{_entreeReactifServiceUrl}/depenses/mois/{annee}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return Ok(apiResponse);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération des dépenses.");
            }
        }
    }
}