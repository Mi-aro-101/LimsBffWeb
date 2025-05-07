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
    [Route("api/depart")]
    public class DepartBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _departURL = "http://localhost:5290/api/depart";

        public DepartBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("ListeDepart")]   
        public async Task<IActionResult> GetDeparts()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_departURL + "/listeDepart");
            if (apiResponse == null) return NotFound();
            apiResponse.HandleResponse<List<DepartDto>>();
            return Ok(apiResponse);
        }

        [HttpGet("destinataireListe")]
        public async Task<IActionResult> GetDestinataire()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_departURL + "/listeDestinataire");
            apiResponse.HandleResponse<List<DestinataireDto>>();
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        [HttpPost("addDepart")]
        public async Task<IActionResult> AddDepart(DepartDto depart)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_departURL}", depart);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<DepartDto>();
                DepartDto departement = (DepartDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }
    }
}