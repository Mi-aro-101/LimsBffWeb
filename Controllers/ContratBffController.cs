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
        private readonly HttpClient _httpClient;
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
            apiResponse.HandleResponse<List<ContratDto>>();
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

    }
}
