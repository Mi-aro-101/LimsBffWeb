using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/receptionpaiementespece")]
    public class ReceptionEspecePaiementBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _especeURL = "http://localhost:5290/api/receptionespecepaiement";

        public ReceptionEspecePaiementBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult> GetReceptionEspecePaiement()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_especeURL);
            apiresponse.HandleResponse<RecuDto>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }

        [HttpPost]
        public async Task<ActionResult> AddReceptionEspece([FromBody] RecuDto recue)
        {
            var response = await _httpClient.PostAsJsonAsync(_especeURL, recue);
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
