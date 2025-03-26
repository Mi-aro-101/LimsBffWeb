using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/encaissement")]
    public class EtatJournalierBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _encaissementURL = "http://localhost:5290/api/etatjournalier";

        public EtatJournalierBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> EncaissementJournalier()
        {
            string requestUri = $"{_encaissementURL}";
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            apiresponse.HandleResponse<List<EncaissementJournalierDto>>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }
    }
}
