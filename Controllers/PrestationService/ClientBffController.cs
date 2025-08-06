using System.Text.Json;
using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/client")]
public class ClientBffController : Controller
{
    private readonly string _clientServiceUrl = "http://localhost:5013/api/client";
    private readonly HttpClient _httpClient;
    public ClientBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetClients()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_clientServiceUrl);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Clients");
        }
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateClient(ClientDto clientDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_clientServiceUrl, clientDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.IsSuccess == true)
        {
            apiResponse.HandleResponse<ClientDto>();
            if (apiResponse.Data == null)
            {
                return BadRequest("Une erreur s'est produite");
            }
            ClientDto client = (ClientDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest($"Une erreur s'est produite lors de la création de l'employé {clientDto.Nom}");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateClient(int id, ClientDto clientDto)
    {
        string requestUri = $"{_clientServiceUrl}/{id}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, clientDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest($"Une erreur s'est produite lors de la mis à jour de donnée : Client {clientDto.Nom}");
        }
        else if (apiResponse?.Data != null)
        {
            return Ok(apiResponse);
        }
        else return BadRequest(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetClient(int id)
    {
        string requestUri = $"{_clientServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest($"Une erreur s'est produite lors de la récupération de donnée du Client {id}");
        }
        return Ok(apiResponse);
    }

    [HttpGet("search")]
    public async Task<ActionResult> SearchClients(string? searchTerm)
    {
        string requestUri = $"{_clientServiceUrl}/search?searchTerm={Uri.EscapeDataString(searchTerm ?? string.Empty)}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la recherche des clients");
        }
        return Ok(apiResponse);
    }
}