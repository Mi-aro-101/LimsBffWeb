using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/versement")]
    public class EtatHebdomadaireBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        private readonly string _versementURL = "http://localhost:5290/api/etathebdomadaire";

        public EtatHebdomadaireBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<ActionResult> AddWeek([FromBody] SemaineDto semaine)
        {
            var response = await _httpClient.PostAsJsonAsync(_versementURL, semaine);
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

        [HttpGet("semaine")]
        public async Task<ActionResult> GetALLweek()
        {
            string requestUri = $"{_versementURL}/semaine";
            Console.WriteLine($"l'url1:{requestUri}");
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            apiresponse.HandleResponse<List<SemaineDto>>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }

        [HttpGet("transfert")]
        public async Task<ActionResult> GetAllVersement()
        {
            string requestUri = $"{_versementURL}/transferer";
             Console.WriteLine($"l'url2:{requestUri}");
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            apiresponse.HandleResponse<List<VersementHebdomadaireDto>>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }
    }
}