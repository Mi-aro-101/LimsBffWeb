using LimsBffWeb.Models;
using LimsBffWeb.Utils;
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
        private readonly string _demandeNoteDebitURL = "http://localhost:5290/api/delaipaiement";

        public DelaiPaiementBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllListeDelaiAsync()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_demandeNoteDebitURL);
            apiresponse.HandleResponse<List<DelaiDto>>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }

        [HttpPost]
        public async Task<ActionResult> AddDelaiPaiementAsync(DelaiDto delai)
        {
            var response = await _httpClient.PostAsJsonAsync(_demandeNoteDebitURL, delai);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<DepartementDto>();
                DepartementDto departement = (DepartementDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }
    }
}
