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
        if(apiResponse == null) return NotFound();
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
            ClientDto client = (ClientDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest(apiResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateClient(int id, ClientDto clientDto)
    {
        string requestUri = $"{_clientServiceUrl}/{id}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, clientDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
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
        if(apiResponse == null) return NotFound();
        return Ok(apiResponse);
    }
}