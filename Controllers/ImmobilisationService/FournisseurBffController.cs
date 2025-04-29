using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LimsUtils.Api;
using System.Net.Http.Json;
using System.Net;
using LimsBffWeb.Models.Immobilisation;

namespace LimsBffWeb.Controllers.ImmobilisationService
{
    [ApiController]
    [Route("api/fournisseurs")]
    public class FournisseurBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        // URL de l'API distante pour les fournisseurs
        private readonly string _fournisseurServiceUrl = "http://localhost:5066/api/fournisseurs";

        public FournisseurBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupérer le total des fournisseurs
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalFournisseurs()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_fournisseurServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();
            return Ok(apiResponse);
        }

        // Récupérer les fournisseurs avec pagination
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetFournisseurs(int position = 1, int pageSize = 10)
        {
            string requestUri = $"{_fournisseurServiceUrl}?position={position}&pageSize={pageSize}";
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            if (apiResponse == null)
                return NotFound();
            return Ok(apiResponse);
        }

        // Récupérer un fournisseur par son ID
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

        // Créer un fournisseur
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateFournisseur(FournisseurDto fournisseurDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_fournisseurServiceUrl, fournisseurDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Created("", apiResponse);
            else
                return BadRequest("Erreur lors de la création du fournisseur.");
        }

        // Mettre à jour un fournisseur
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateFournisseur(int id, FournisseurDto fournisseurDto)
        {
            string requestUri = $"{_fournisseurServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, fournisseurDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Erreur lors de la mise à jour.");
        }

        // Supprimer un fournisseur
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteFournisseur(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_fournisseurServiceUrl}/{id}");
            if (response.IsSuccessStatusCode)
                return Ok(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Fournisseur supprimé avec succès.",
                    StatusCode = 200
                });
            else
                return BadRequest("Erreur lors de la suppression.");
        }
    }
}