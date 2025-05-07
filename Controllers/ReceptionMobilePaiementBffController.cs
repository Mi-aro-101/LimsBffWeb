using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/receptionpaiementmobile")]
    public class ReceptionMobilePaiementBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _mobileURL = "http://localhost:5290/api/receptionmobilepaiement";

        public ReceptionMobilePaiementBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult> GetReceptionMobilePaiement()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_mobileURL);
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<DashboardPaiementDto>();
            return Ok(apiresponse);
        }

        [HttpPost]
        public async Task<ActionResult> AddReceptionMobile([FromBody] RecuDto recue)
        {
            var response = await _httpClient.PostAsJsonAsync(_mobileURL, recue);
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

         [HttpGet("Confirmer")]
        public async Task<ActionResult> GetReceptionMobilePaiementAConfirmer()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_mobileURL+"/AConfirmer");
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<RecuDto>();
            return Ok(apiresponse);
        }
    }
}
