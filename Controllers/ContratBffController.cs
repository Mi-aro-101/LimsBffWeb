using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/contrat")]
    public class ContratBffController : ControllerBase
    {
        /*private readonly HttpClient _httpClient;
        private readonly string _contratURL = "http://localhost:5290/api/contrat";

        public ContratBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("ajout")]
        public async Task<ActionResult> AddContrat(ContratDto contrat)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_contratURL}/new", contrat);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<ContratDto>();
                ContratDto departement = (ContratDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllContrat(int position, int pagesize)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_contratURL + "?position=" + position + "&pageSize=" + pagesize);
            if (apiResponse == null) return NotFound();
            apiResponse.HandleResponse<List<ContratDto>>();
            return Ok(apiResponse);
        }

        [HttpGet("{id_partenaire}")]
        public async Task<ActionResult> GetContratModifier(int id_partenaire)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_contratURL}/{id_partenaire}");
            apiResponse.HandleResponse<ContratDto>();
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        [HttpPut("{id_partenaire}/{id_contrat}")]
        public async Task<ActionResult> ContratModifier(int id_partenaire, int id_contrat, [FromBody] ContratDto contrat)
        {
            string requestUri = $"{_contratURL}/{id_partenaire}/{id_contrat}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, contrat);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.IsSuccess == false || apiResponse == null)
            {
                return BadRequest("Une erreur s'est produite lors de la mis à jour des données : destinataire");
            }
            else if (apiResponse?.Data != null)
            {
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }*/
    }
}
