using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models;
using System.Text.Json;
using LimsUtils.Api;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/recette/previsionnelle")]
public class RecettePrevisionnelle : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _recettePrevisionnelleServiceUrl = "http://localhost:5000/api/recette/previsionnelle";

    public RecettePrevisionnelle(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost]
    public async Task<ActionResult> CreateRecettePrevisionnelle(RecettePrevisionnelleDto recettePrevisionnelle)
    {
        var response = await _httpClient.PostAsJsonAsync(_recettePrevisionnelleServiceUrl, recettePrevisionnelle);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<RecettePrevisionnelleDto>();
            RecettePrevisionnelleDto recettePrevisionnelleDto = (RecettePrevisionnelleDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest("Data is null");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetRecettePrevisionnelle(int id)
    {
        string requestUri = $"{_recettePrevisionnelleServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        apiResponse.HandleResponse<RecettePrevisionnelleDto>();
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }
}