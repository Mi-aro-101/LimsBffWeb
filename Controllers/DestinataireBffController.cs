using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/destinataire")]
    public class DestinataireBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _destinataireURL = "http://localhost:5290/api/destinataire";

        public DestinataireBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("ajout")]
        public async Task<ActionResult> AddDestinataire(DestinataireDto contrat)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_destinataireURL}/new", contrat);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<DestinataireDto>();
                DestinataireDto departement = (DestinataireDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllContrat(int position, int pagesize)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_destinataireURL + "?position=" + position + "&pageSize=" + pagesize);
            apiResponse.HandleResponse<List<DestinataireDto>>();
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        [HttpGet("{id_destinataire}")]
        public async Task<ActionResult> GetDestinataireModification(int id_destinataire)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_destinataireURL}/{id_destinataire}");
            apiResponse.HandleResponse<DestinataireDto>();
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        [HttpPut("{Id_destinataire}")]
        public async Task<ActionResult> ModificationDestinataire(int Id_destinataire, [FromBody] DestinataireDto destinataire)
        {
            string requestUri = $"{_destinataireURL}/{Id_destinataire}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, destinataire);
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