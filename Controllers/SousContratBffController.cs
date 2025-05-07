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
    [Route("api/souscontrat")]
    public class SousContratBffController : ControllerBase
    {
         private readonly HttpClient _httpClient;
        private readonly string _souscontratURL = "http://localhost:5290/api/souscontrat";

        public SousContratBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("ListeAllSousContrat")]
        public async Task<ActionResult> GetAllSousContratAsync()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_souscontratURL+"/ListeSousContrat");
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<List<SousContratDto>>();
            return Ok(apiresponse);
        }

        [HttpGet("ListeSousContratApayer")]
        public async Task<ActionResult> GetAllSousContratApayerAsync()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_souscontratURL+"/ListeSousContratApayer");
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<List<SousContratDto>>();
            return Ok(apiresponse);
        }

        [HttpPut("UpdateSousContrat/{id_sous_contrat}")]
        public async Task<ActionResult> UpdateSousContrat(int id_sous_contrat)
        {
            var response = await _httpClient.PutAsync($"{_souscontratURL}/UpdateSousContrat/{id_sous_contrat}", null);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Update forwarded successfully", BackendResponse = responseContent });
            }
            else
            {
                return BadRequest("Ohatran'ny nisy olana tao a");
            }
        }
       
        [HttpPost]        
        public async Task<ActionResult> NewPaiementSousContrat([FromBody] SousContratDto contrat)
        {
            var response = await _httpClient.PostAsJsonAsync(_souscontratURL, contrat);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<SousContratDto>();
                SousContratDto departement = (SousContratDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }        
    }
}