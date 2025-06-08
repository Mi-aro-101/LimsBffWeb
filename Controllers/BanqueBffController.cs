using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/banque")]
    public class BanqueBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _banqueURL = "http://localhost:5290/api/banque";

        public BanqueBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("ajout")]
        public async Task<ActionResult> AddBanque(BanqueDto banque)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_banqueURL}/new", banque);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<BanqueDto>();
                BanqueDto banques = (BanqueDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBanque(int position, int pagesize)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_banqueURL + "?position=" + position + "&pageSize=" + pagesize);
            if (apiResponse == null) return NotFound();
            apiResponse.HandleResponse<List<BanqueDto>>();
            return Ok(apiResponse);
        }

        [HttpGet("{id_banque}")]
        public async Task<ActionResult> GetBanqueModification(int id_banque)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_banqueURL}/{id_banque}");
            if (apiResponse == null) return NotFound();
            apiResponse.HandleResponse<BanqueDto>();
            return Ok(apiResponse);
        }

        [HttpPut("{id_banque}")]
        public async Task<ActionResult> ModificationBanque(int id_banque, [FromBody] BanqueDto banque)
        {
            string requestUri = $"{_banqueURL}/{id_banque}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, banque);
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
        }
    }
}