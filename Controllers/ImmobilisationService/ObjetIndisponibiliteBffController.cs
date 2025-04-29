using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LimsUtils.Api;
using System.Net.Http.Json;
using System.Net;
using LimsBffWeb.Models.Immobilisation; // Assurez-vous que ce namespace contient ObjetIndisponibiliteDto
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.ImmobilisationService
{
    [ApiController]
    [Route("api/objets-indisponibilite")]
    public class ObjetIndisponibiliteBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _objetIndisponibiliteServiceUrl = "http://localhost:5066/api/objets-indisponibilite"; // Ajustez l'URL selon votre configuration

        public ObjetIndisponibiliteBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupérer le total des objets d'indisponibilité
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalObjetsIndisponibilite()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_objetIndisponibiliteServiceUrl + "/total");
            if (apiResponse == null)
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la récupération du total",
                    StatusCode = 404
                });
            return Ok(apiResponse);
        }

        // Récupérer les objets d'indisponibilité avec pagination
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetObjetsIndisponibilite(int position = 1, int pageSize = 10)
        {
            string requestUri = $"{_objetIndisponibiliteServiceUrl}?position={position}&pageSize={pageSize}";
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            if (apiResponse == null)
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la récupération des objets",
                    StatusCode = 404
                });
            return Ok(apiResponse);
        }

        // Récupérer un objet d'indisponibilité par ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetObjetIndisponibilite(int id)
        {
            string requestUri = $"{_objetIndisponibiliteServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Objet d'indisponibilité non trouvé",
                    StatusCode = 404
                });
            }
            else if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la récupération de l'objet",
                    StatusCode = (int)response.StatusCode
                });
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }

        // Créer un nouvel objet d'indisponibilité
       [HttpPost]
public async Task<ActionResult<ApiResponse>> CreateObjetIndisponibilite([FromBody] ObjetIndisponibiliteDto objetIndisponibiliteDto)
{
    var response = await _httpClient.PostAsJsonAsync(_objetIndisponibiliteServiceUrl, objetIndisponibiliteDto);
    using var responseStream = await response.Content.ReadAsStreamAsync();
    var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);

    if (response.IsSuccessStatusCode && apiResponse?.Data != null)
    {
        // Retourner un statut 201 avec les données, sans générer une URL via CreatedAtAction
        return StatusCode(201, apiResponse);
    }
    else
    {
        return BadRequest(new ApiResponse
        {
            Data = null,
            ViewBag = null,
            IsSuccess = false,
            Message = "Erreur lors de la création de l'objet d'indisponibilité",
            StatusCode = (int)response.StatusCode
        });
    }
}

        // Mettre à jour un objet d'indisponibilité
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateObjetIndisponibilite(int id, [FromBody] ObjetIndisponibiliteDto objetIndisponibiliteDto)
        {
            string requestUri = $"{_objetIndisponibiliteServiceUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(requestUri, objetIndisponibiliteDto);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);

            if (response.IsSuccessStatusCode && apiResponse?.Data != null)
            {
                return Ok(apiResponse);
            }
            else
            {
                return BadRequest(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la mise à jour de l'objet d'indisponibilité",
                    StatusCode = (int)response.StatusCode
                });
            }
        }

        // Supprimer un objet d'indisponibilité
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteObjetIndisponibilite(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_objetIndisponibiliteServiceUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Objet d'indisponibilité supprimé avec succès",
                    StatusCode = 200
                });
            }
            else
            {
                return StatusCode((int)response.StatusCode, new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la suppression de l'objet d'indisponibilité",
                    StatusCode = (int)response.StatusCode
                });
            }
        }
    }
}