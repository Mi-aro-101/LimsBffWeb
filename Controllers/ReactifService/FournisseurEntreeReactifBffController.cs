using Microsoft.AspNetCore.Mvc;
using LimsUtils.Api;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace LimsBffWeb.Controllers.ReactifService
{
    [ApiController]
    // Modification du préfixe de route pour éviter le conflit
    [Route("/api/fournisseurs-entree")]
    public class FournisseurEntreeReactifBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _fournisseurServiceUrl = "http://localhost:5073/api/fournisseurs"; // URL de l'API distante

        public FournisseurEntreeReactifBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/fournisseurs-entree/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalFournisseurs()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_fournisseurServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/fournisseurs-entree?position=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetFournisseurs(int position = 1, int pageSize = 10)
        {
            if (position < 1)
                position = 1;
            if (pageSize < 1)
                pageSize = 10;

            string requestUrl = $"{_fournisseurServiceUrl}?position={position}&pageSize={pageSize}";
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUrl);
            if (apiResponse == null)
                return NotFound();

            return Ok(apiResponse);
        }

        // GET api/fournisseurs-entree/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetFournisseur(int id)
        {
            string requestUrl = $"{_fournisseurServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound("Fournisseur non trouvé.");
            else if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération du fournisseur.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }
    }
}
