using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/paiementparmobile")]
    public class MobilePaiementBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _mobileURL = "http://localhost:5290/api/mobilepaiement";

        public MobilePaiementBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id_etat_decompte}")]
        public async Task<IActionResult> GetMobilePaiement(int id_etat_decompte)
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_mobileURL}/{id_etat_decompte}");
            apiresponse.HandleResponse<List<PaiementDto>>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddPaiementParMobile([FromBody] PaiementDto paiement)
        {
            var response = await _httpClient.PostAsJsonAsync(_mobileURL, paiement);
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
