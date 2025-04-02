using System.Text.Json;
using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/chiffre/affaire")]
public class ChiffreAffaireController : Controller
{
    private readonly string _chiffreAffaireUrl = "http://localhost:5013/api/chiffre/affaire";

    private readonly HttpClient _httpClient;

    public ChiffreAffaireController(HttpClient httpClient)
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
}