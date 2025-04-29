using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.ReactifService; // Pour le DTO ObjetSortieReactifDto
using LimsUtils.Api; // Pour ApiResponse
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.ReactifService
{
    [ApiController]
    [Route("api/objetSortieReactif")]
    public class ObjetSortieReactifBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        // URL de l'API back pour les objets de sortie réactif
        private readonly string _objetSortieReactifServiceUrl = "http://localhost:5073/api/objetSortieReactif";

        public ObjetSortieReactifBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/objetSortieReactif/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalObjetsSortieReactif()
        {
            var url = $"{_objetSortieReactifServiceUrl}/total";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erreur lors de la récupération du total : {response.ReasonPhrase}");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // GET api/objetSortieReactif?position=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetObjetsSortieReactif(int position = 1, int pageSize = 10)
        {
            var url = $"{_objetSortieReactifServiceUrl}?position={position}&pageSize={pageSize}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erreur lors de la récupération des objets de sortie réactif : {response.ReasonPhrase}");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // GET api/objetSortieReactif/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetObjetSortieReactif(int id)
        {
            var url = $"{_objetSortieReactifServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Objet de sortie réactif non trouvé");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erreur lors de la récupération de l'objet de sortie réactif : {response.ReasonPhrase}");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // POST api/objetSortieReactif
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateObjetSortieReactif([FromBody] ObjetSortieReactifDto objetSortieReactifDto)
        {
            var url = _objetSortieReactifServiceUrl;
            var response = await _httpClient.PostAsJsonAsync(url, objetSortieReactifDto);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Une erreur est survenue lors de la création de l'objet de sortie réactif.");
            }

            // Pour gérer le champ Data qui peut être désérialisé en JsonElement
            var jsonString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (apiResponse != null && apiResponse.Data is JsonElement element)
            {
                // Désérialisation manuelle de Data en ObjetSortieReactifDto
                var createdObjet = element.Deserialize<ObjetSortieReactifDto>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (createdObjet != null)
                {
                    return CreatedAtAction(nameof(GetObjetSortieReactif), new { id = createdObjet.IdObjetSortieReactif }, apiResponse);
                }
            }

            return BadRequest("Une erreur est survenue lors de la création de l'objet de sortie réactif.");
        }

        // PUT api/objetSortieReactif/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateObjetSortieReactif(int id, [FromBody] ObjetSortieReactifDto objetSortieReactifDto)
        {
            var url = $"{_objetSortieReactifServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(url, objetSortieReactifDto);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Objet de sortie réactif non trouvé.");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Une erreur est survenue lors de la mise à jour de l'objet de sortie réactif.");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // DELETE api/objetSortieReactif/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteObjetSortieReactif(int id)
        {
            var url = $"{_objetSortieReactifServiceUrl}/{id}";
            var response = await _httpClient.DeleteAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Objet de sortie réactif non trouvé.");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Une erreur est survenue lors de la suppression de l'objet de sortie réactif.");
            }

            return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Objet de sortie réactif supprimé avec succès.",
                StatusCode = 200
            });
        }
    }
}
