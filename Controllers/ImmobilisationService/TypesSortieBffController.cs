using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models.ReactifService; // Pour le DTO TypeSortieDto
using LimsUtils.Api; // Pour ApiResponse
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.ImmobilisationService
{
    [ApiController]
    [Route("api/typesSortie")]
    public class TypesSortieBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        // L'URL de l'API back est définie ici et n'est utilisée que dans ce contrôleur
        private readonly string _typeSortieServiceUrl = "http://localhost:5073/api/typesSortie";

        public TypesSortieBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/typesSortie/total
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalTypesSortie()
        {
            var url = $"{_typeSortieServiceUrl}/total";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erreur lors de la récupération du total : {response.ReasonPhrase}");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // GET api/typesSortie?position=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetTypesSortie(int position = 1, int pageSize = 10)
        {
            var url = $"{_typeSortieServiceUrl}?position={position}&pageSize={pageSize}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erreur lors de la récupération des types de sortie : {response.ReasonPhrase}");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // GET api/typesSortie/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetTypeSortie(int id)
        {
            var url = $"{_typeSortieServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Type de sortie non trouvé");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erreur lors de la récupération du type de sortie : {response.ReasonPhrase}");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // POST api/typesSortie
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateTypeSortie([FromBody] TypeSortieDto typeSortieDto)
        {
            var url = _typeSortieServiceUrl;
            var response = await _httpClient.PostAsJsonAsync(url, typeSortieDto);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Une erreur est survenue lors de la création du type de sortie.");
            }

            // Pour gérer le champ Data qui peut être désérialisé en JsonElement
            var jsonString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (apiResponse != null && apiResponse.Data is JsonElement element)
            {
                // Désérialisation manuelle de Data en TypeSortieDto
                var createdTypeSortie = element.Deserialize<TypeSortieDto>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (createdTypeSortie != null)
                {
                    return CreatedAtAction(nameof(GetTypeSortie), new { id = createdTypeSortie.IdTypeSortie }, apiResponse);
                }
            }

            return BadRequest("Une erreur est survenue lors de la création du type de sortie.");
        }

        // PUT api/typesSortie/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateTypeSortie(int id, [FromBody] TypeSortieDto typeSortieDto)
        {
            var url = $"{_typeSortieServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(url, typeSortieDto);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Type de sortie non trouvé.");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Une erreur est survenue lors de la mise à jour du type de sortie.");
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // DELETE api/typesSortie/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteTypeSortie(int id)
        {
            var url = $"{_typeSortieServiceUrl}/{id}";
            var response = await _httpClient.DeleteAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Type de sortie non trouvé.");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Une erreur est survenue lors de la suppression du type de sortie.");
            }

            return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Type de sortie supprimé avec succès.",
                StatusCode = 200
            });
        }
    }
}
