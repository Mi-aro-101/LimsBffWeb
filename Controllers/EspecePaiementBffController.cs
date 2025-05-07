using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/paiementenespece")]
    public class EspecePaiementBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _especeURL = "http://localhost:5290/api/especepaiement";

        public EspecePaiementBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id_etat_decompte}")]
        public async Task<ActionResult> GetEspecePaiement(int id_etat_decompte)
        {
            string request = $"{_especeURL}/{id_etat_decompte}";
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(request);
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<List<PaiementDto>>();
            return Ok(apiresponse);
        }

        [HttpPost]
        public async Task<ActionResult> AddPaiementEnEspece([FromBody] PaiementDto paiement)
        {
            var response = await _httpClient.PostAsJsonAsync(_especeURL, paiement);
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
    }
}
