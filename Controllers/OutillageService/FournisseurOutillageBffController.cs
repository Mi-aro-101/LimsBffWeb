using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.Outillage; // Namespace supposé pour les DTOs du BFF
using LimsUtils.Api;
using System.Net;
using System.Text.Json;

namespace LimsBffWeb.Controllers.OutillageService
{
    [ApiController]
    [Route("/api/fournisseur")]
    public class FournisseurOutillageBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _fournisseurServiceUrl = "http://localhost:5213/api/fournisseur";

        public FournisseurOutillageBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupère le nombre total de fournisseurs
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalFournisseurs()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_fournisseurServiceUrl + "/total");
            if (apiResponse == null)
            {
                return NotFound();
            }
            return Ok(apiResponse);
        }

        // Récupère une liste paginée de fournisseurs
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetFournisseurs(int position = 1, int pageSize = 10)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(
                _fournisseurServiceUrl + $"?position={position}&pageSize={pageSize}"
            );
            if (apiResponse == null)
            {
                return NotFound();
            }
            return Ok(apiResponse);
        }

        // Récupère un fournisseur par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFournisseur(int id)
        {
            string requestUri = $"{_fournisseurServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Fournisseur non trouvé");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération du fournisseur");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }
    }
}