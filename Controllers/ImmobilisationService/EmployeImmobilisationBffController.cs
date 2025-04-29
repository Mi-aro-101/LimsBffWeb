using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LimsUtils.Api;
using System.Net;
using LimsBffWeb.Models.Immobilisation;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LimsBffWeb.Controllers.ImmobilisationService
{
    [ApiController]
    [Route("/api/employes")]
    public class EmployeImmobilisationBffController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _employeServiceUrl = "http://localhost:5066/api/employes";

        public EmployeImmobilisationBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupère le nombre total d'employés
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalEmployes()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_employeServiceUrl + "/total");
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        // Récupère une liste paginée d'employés
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetEmployes(int position = 1, int pageSize = 10)
        {
            var uri = $"{_employeServiceUrl}?position={position}&pageSize={pageSize}";
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(uri);
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        // Recherche d'employés
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse>> SearchEmployes([FromQuery] string searchTerm = "")
        {
            var uri = $"{_employeServiceUrl}/search?searchTerm={WebUtility.UrlEncode(searchTerm)}";
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(uri);
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        // Récupère un employé par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetEmploye(int id)
        {
            var requestUri = $"{_employeServiceUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new ApiResponse { Data = null, ViewBag = null, IsSuccess = false, Message = "Employé non trouvé.", StatusCode = 404 });

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de l'employé.");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return Ok(apiResponse);
        }
    }
}
