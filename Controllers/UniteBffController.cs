using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.ReactifService; // Assurez-vous que UniteDto est défini ici ou ajustez le namespace
using LimsUtils.Api;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/unites")]
    public class UnitesBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        // URL de l'API back pour les unités (exclusif à ce contrôleur)
        private readonly string _uniteServiceUrl = "http://localhost:5073/api/unites";

        public UnitesBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/unites/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalUnites()
        {
            var url = $"{_uniteServiceUrl}/total";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erreur lors de la récupération du total des unités : {response.ReasonPhrase}");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // GET api/unites?position=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetUnites(int position = 1, int pageSize = 10)
        {
            var url = $"{_uniteServiceUrl}?position={position}&pageSize={pageSize}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erreur lors de la récupération des unités : {response.ReasonPhrase}");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // GET api/unites/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetUnite(int id)
        {
            var url = $"{_uniteServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Unité non trouvée");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erreur lors de la récupération de l'unité : {response.ReasonPhrase}");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // POST api/unites
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateUnite([FromBody] UniteDto uniteDto)
        {
            var url = _uniteServiceUrl;
            var response = await _httpClient.PostAsJsonAsync(url, uniteDto);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Une erreur est survenue lors de la création de l'unité.");
            }

            // Lecture brute de la réponse pour gérer la désérialisation du champ Data
            var jsonString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (apiResponse != null && apiResponse.Data is JsonElement element)
            {
                var createdUnite = element.Deserialize<UniteDto>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (createdUnite != null)
                {
                    return CreatedAtAction(nameof(GetUnite), new { id = createdUnite.IdUnite }, apiResponse);
                }
            }
            return BadRequest("Une erreur est survenue lors de la création de l'unité.");
        }

        // PUT api/unites/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateUnite(int id, [FromBody] UniteDto uniteDto)
        {
            var url = $"{_uniteServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(url, uniteDto);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Unité non trouvée.");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Une erreur est survenue lors de la mise à jour de l'unité.");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // DELETE api/unites/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteUnite(int id)
        {
            var url = $"{_uniteServiceUrl}/{id}";
            var response = await _httpClient.DeleteAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Unité non trouvée.");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Une erreur est survenue lors de la suppression de l'unité.");
            }

            return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Unité supprimée avec succès.",
                StatusCode = 200
            });
        }
    }
}

