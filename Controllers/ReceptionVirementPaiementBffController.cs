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
            apiresponse.HandleResponse<RecuDto>();
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
    }
}
