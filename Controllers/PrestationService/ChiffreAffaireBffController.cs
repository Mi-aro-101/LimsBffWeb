using System.Text.Json;
using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/chiffre/affaire")]
public class ChiffreAffaireBffController : Controller
{
    private readonly string _chiffreAffaireUrl = "http://localhost:5013/api/chiffre/affaire";

    private readonly HttpClient _httpClient;

    public ChiffreAffaireBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("mensuel")]
    public async Task<ActionResult<ApiResponse>> GetMensuelChiffreAffaire([FromBody] ChiffreAffaireDto? chiffreAffaire)
    {
        string requestUri = _chiffreAffaireUrl + "/mensuel";
        var response = await _httpClient.PostAsJsonAsync(requestUri, chiffreAffaire);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        return Ok(apiResponse);
    }

    [HttpPost("annuel")]
    public async Task<ActionResult<ApiResponse>> GetAnnuelChiffreAffaire([FromBody] ChiffreAffaireDto? chiffreAffaire)
    {
        string requestUri = _chiffreAffaireUrl + "/annuel";
        var response = await _httpClient.PostAsJsonAsync(requestUri, chiffreAffaire);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        return Ok(apiResponse);
    }

    [HttpPost("journalier")]
    public async Task<ActionResult<ApiResponse>> GetAnnuelChiffreJournalier([FromBody] ChiffreAffaireDto? chiffreAffaire)
    {
        string requestUri = _chiffreAffaireUrl + "/journalier";
        var response = await _httpClient.PostAsJsonAsync(requestUri, chiffreAffaire);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        return Ok(apiResponse);
    }

    [HttpPost("departement/mensuel")]
    public async Task<ActionResult<ApiResponse>> GetChiffreAffaireParDepartementMensuel([FromBody] ChiffreAffaireDto? chiffreAffaire)
    {
        string requestUri = _chiffreAffaireUrl + "/departement/mensuel";
        var response = await _httpClient.PostAsJsonAsync(requestUri, chiffreAffaire);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        return Ok(apiResponse);
    }

    [HttpPost("departement/annuel")]
    public async Task<ActionResult<ApiResponse>> GetChiffreAffaireParDepartementAnnuel([FromBody] ChiffreAffaireDto? chiffreAffaire)
    {
        string requestUri = _chiffreAffaireUrl + "/departement/annuel";
        var response = await _httpClient.PostAsJsonAsync(requestUri, chiffreAffaire);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        return Ok(apiResponse);
    }
}