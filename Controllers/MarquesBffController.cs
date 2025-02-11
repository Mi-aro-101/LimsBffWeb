using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models;          // Crée un DTO MarqueDto adapté si nécessaire
using System.Text.Json;
using LimsUtils.Api;
using System.Net.Http.Json;
using System.Net;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/marque")]
    public class MarquesBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        // URL de ton API LimsImmobilisationService pour les marques
        private readonly string _marqueServiceUrl = "http://localhost:5066/api/marques";

        public MarquesBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupérer le total des marques
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalMarques()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_marqueServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound();
            return Ok(apiResponse);
        }

        // Récupérer les marques avec pagination
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetMarques(int position = 1, int pageSize = 10)
        {
            string requestUri = $"{_marqueServiceUrl}?position={position}&pageSize={pageSize}";
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            if (apiResponse == null)
                return NotFound();
            return Ok(apiResponse);
        }

        // Récupérer une marque par son ID
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ApiResponse>> GetMarque(int id)
        //{
        //    string requestUri = $"{_marqueServiceUrl}/{id}";
        //    var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        //    if (apiResponse == null)
        //        return NotFound();
        //    return Ok(apiResponse);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMarque(int id)
        {
            string requestUri = $"{_marqueServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Marque non trouvée");
            }
            else if (!response.IsSuccessStatusCode)
            {
                // Gérer d'autres codes d'erreur si nécessaire
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de la marque");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }


        // Créer une marque
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMarque(MarqueDto marqueDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_marqueServiceUrl, marqueDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Created("", apiResponse);
            else
                return BadRequest("Erreur lors de la création de la marque.");
        }

        // Mettre à jour une marque
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateMarque(int id, MarqueDto marqueDto)
        {
            string requestUri = $"{_marqueServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, marqueDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
                return Ok(apiResponse);
            else
                return BadRequest("Erreur lors de la mise à jour.");
        }

        // Supprimer une marque
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteMarque(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_marqueServiceUrl}/{id}");
            if (response.IsSuccessStatusCode)
                return Ok(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Marque supprimée avec succès.",
                    StatusCode = 200
                });
            else
                return BadRequest("Erreur lors de la suppression.");
        }
    }
}
//using Microsoft.AspNetCore.Mvc;
//using LimsBffWeb.Models;
//using System.Text.Json;
//using LimsUtils.Api;

//namespace LimsBffWeb.Controllers;

//[ApiController]
//[Route("/api/marque")] // Note le singulier pour rester cohérent avec "/api/employe"
//public class MarqueBFFController : Controller
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _marqueServiceUrl = "http://localhost:5066/api/marques"; // URL de TON API

//    public MarqueBFFController(HttpClient httpClient)
//    {
//        _httpClient = httpClient;
//    }

//    // GET /api/marque/total
//    [HttpGet("total")]
//    public async Task<ActionResult<ApiResponse>> GetTotalMarques()
//    {
//        var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_marqueServiceUrl}/total");
//        return apiResponse != null ? Ok(apiResponse) : NotFound();
//    }

//    // GET /api/marque?position=1&pageSize=10
//    [HttpGet]
//    public async Task<ActionResult<ApiResponse>> GetMarques(int position, int pageSize)
//    {
//        var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(
//            $"{_marqueServiceUrl}?position={position}&pageSize={pageSize}"
//        );
//        return apiResponse != null ? Ok(apiResponse) : NotFound();
//    }

//    // GET /api/marque/{id}
//    [HttpGet("{id}")]
//    public async Task<ActionResult> GetMarque(int id)
//    {
//        var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>($"{_marqueServiceUrl}/{id}");
//        return apiResponse != null ? Ok(apiResponse) : NotFound();
//    }

//    // POST /api/marque
//    [HttpPost]
//    public async Task<ActionResult> CreateMarque(MarqueDto marqueDto)
//    {
//        var response = await _httpClient.PostAsJsonAsync(_marqueServiceUrl, marqueDto);
//        var responseStream = await response.Content.ReadAsStreamAsync();
//        var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
//        return apiResponse?.Data != null ? Ok(apiResponse) : BadRequest("Erreur lors de la création");
//    }

//    // PUT /api/marque/{id}
//    [HttpPut("{id}")]
//    public async Task<ActionResult> UpdateMarque(int id, MarqueDto marqueDto)
//    {
//        var response = await _httpClient.PutAsJsonAsync($"{_marqueServiceUrl}/{id}", marqueDto);
//        var responseStream = await response.Content.ReadAsStreamAsync();
//        var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
//        return apiResponse?.Data != null ? Ok(apiResponse) : BadRequest("Erreur lors de la mise à jour");
//    }

//    // DELETE /api/marque/{id}
//    [HttpDelete("{id}")]
//    public async Task<ActionResult> DeleteMarque(int id)
//    {
//        var response = await _httpClient.DeleteAsync($"{_marqueServiceUrl}/{id}");
//        return response.IsSuccessStatusCode ? NoContent() : BadRequest("Erreur lors de la suppression");
//    }
//}