using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/paiementparvirement")]
    public class VirementPaiementBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _virementURL = "http://localhost:5290/api/virementpaiement";

        public VirementPaiementBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id_etat_decompte}")]
        public async Task<ActionResult> GetVirementPaiement(int id_etat_decompte)
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_virementURL}/{id_etat_decompte}");
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<List<PaiementDto>>();
            return Ok(apiresponse);
        }

        [HttpPost]
        public async Task<ActionResult> AddPaiementParVirement([FromBody] PaiementDto paiement)
        {
            var response = await _httpClient.PostAsJsonAsync(_virementURL, paiement);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<PaiementDto>();
                PaiementDto departement = (PaiementDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }

        [HttpGet("ListeVirement")]
        public async Task<ActionResult> GetVirementAllApayer()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_virementURL}/VirementListe");
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<List<PaiementDto>>();
            return Ok(apiresponse);
        }
    }
}
