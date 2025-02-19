using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/delailimite")]
    public class DelaiPaiementBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _delaiURL = "http://localhost:5290/api/delaipaiement";

        public DelaiPaiementBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("ListeAllDelai")]
        public async Task<ActionResult> GetAllListeDelaiAsync()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_delaiURL);
            apiresponse.HandleResponse<List<PaiementDto>>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }

        [HttpGet("VerificationDelai/{id_etat_decompte}")]
        public async Task<ActionResult> GetVerificationDelaiAccorder(int id_etat_decompte)
        {
            string requestUri = $"{_delaiURL}/DelaiAccorder/{id_etat_decompte}";
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            apiresponse.HandleResponse<List<PaiementDto>>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }

        [HttpPost]
        public async Task<ActionResult> AddDelaiPaiementAsync(PaiementDto delai)
        {
            var response = await _httpClient.PostAsJsonAsync(_delaiURL, delai);
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
