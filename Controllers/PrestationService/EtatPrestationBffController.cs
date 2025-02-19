using System.Text.Json;
using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/etat/prestation")]
public class EtatPrestationController : Controller
{
    private readonly string _clientServiceUrl = "http://localhost:5013/api/etat/prestation";
    private readonly HttpClient _httpClient;

    public EtatPrestationController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetEtatsPrestation()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_clientServiceUrl);
        if(apiResponse == null) return NotFound();
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetEtatPrestation(int id)
    {
        string requestUri = $"{_clientServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if(apiResponse == null) return NotFound();
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateEtatPrestation(EtatPrestationDto etatPrestationDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_clientServiceUrl, etatPrestationDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.IsSuccess == true)
        {
            apiResponse.HandleResponse<EtatPrestationDto>();
            EtatPrestationDto etatPrestation = (EtatPrestationDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest(apiResponse);
    }
}