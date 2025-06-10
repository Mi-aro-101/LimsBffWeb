using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/receptionpaiementvirement")]
    public class ReceptionVirementPaiementBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _virementURL = "http://localhost:5290/api/receptionvirementpaiement";

        public ReceptionVirementPaiementBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult> GetReceptionVirementPaiement()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_virementURL);
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<DashboardPaiementDto>();
            return Ok(apiresponse);
        }
        
        [HttpGet("listeBanque")]
        public async Task<ActionResult> GetListeBanque()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_virementURL}/banqueListe");
            apiresponse.HandleResponse<List<BanqueDto>>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }

        [HttpPost]
        public async Task<ActionResult> AddReceptionVirement([FromBody] RecuDto recue)
        {
            var response = await _httpClient.PostAsJsonAsync(_virementURL, recue);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<RecuDto>();
                RecuDto departement = (RecuDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }

        [HttpGet("Confirmer/{id_etat_decompte}")]
        public async Task<ActionResult> GetReceptionVirementPaiementAConfirmer(int id_etat_decompte)
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_virementURL}/AConfirmer/{id_etat_decompte}");
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<RecuDto>();
            return Ok(apiresponse);
        }
        
    }
}
