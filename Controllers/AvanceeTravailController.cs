using System.Text.Json;
using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/avancee/travail")]
public class AvanceeTravailController : Controller
{
    private HttpClient _httpClient;
    private readonly string _avanceeTravailServiceUrl = "http://localhost:5126/api/avancee/travail";

    public AvanceeTravailController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<ActionResult> GetAvanceeTravaux()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_avanceeTravailServiceUrl);
        if (apiResponse == null) return NotFound();
        apiResponse.HandleResponse<List<AvanceeTravailDto>>();
        
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAvanceeTravailDetails(int id)
    {
        string requestUri = $"{_avanceeTravailServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse == null) return NotFound("Service non actif pour le moment");
        apiResponse.HandleResponse<AvanceeTravailDto>();
        
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAvanceeTravail([FromBody] AvanceeTravailDto avanceeTravail)
    {
        var response = await _httpClient.PostAsJsonAsync(_avanceeTravailServiceUrl, avanceeTravail);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data!= null)
        {
            apiResponse.HandleResponse<AvanceeTravailDto>();
            AvanceeTravailDto avanceeTravailDto = (AvanceeTravailDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }

}