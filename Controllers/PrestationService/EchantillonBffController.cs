using System.Text.Json;
using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/echantillon")]
public class EchantillonBffController : Controller
{
     private readonly string _clientServiceUrl = "http://localhost:5013/api/echantillon";
    private readonly HttpClient _httpClient;

    public EchantillonBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateTravailAndCheck(int idTravail, int idPrestation)
    {
        string fullUrl = _clientServiceUrl + $"?idTravail={idTravail}&idPrestation={idPrestation}";
        var response = await _httpClient.PutAsync(fullUrl, null);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            return Ok(apiResponse);
        }
        else return BadRequest("Une erreur s'est produite");
    }
    [HttpPost("qr/{id}")]
    public async Task<ActionResult<ApiResponse>> FicheTravailPdf(int id)
    {
        string fullUrl = _clientServiceUrl+$"/qr/{id}";
        var response = await _httpClient.PostAsJsonAsync(fullUrl, id);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Error generating PDF");

        var stream = await response.Content.ReadAsStreamAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/pdf";
        var fileName = response.Content.Headers.ContentDisposition?.FileName ?? $"Echantillon_{id}.pdf";

        return File(stream, contentType, fileName);
    }

    [HttpGet("{reference}")]
    public async Task<ActionResult<ApiResponse>> GetEchantillonByReference(string reference)
    {
        string requestUri = $"{_clientServiceUrl}/{reference}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);

        return Ok(apiResponse);
    }
}