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
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Etat prestations");
        }
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetEtatPrestation(int id)
    {
        string requestUri = $"{_clientServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest($"Une erreur s'est produite lors de la récupération de donnée : Etat prestation {id}");
        }
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
            if(apiResponse.Data == null)
            {
                return BadRequest("Une erreur s'est produite lors de la création de l'état prestation");
            }
            EtatPrestationDto etatPrestation = (EtatPrestationDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest(apiResponse);
    }
}