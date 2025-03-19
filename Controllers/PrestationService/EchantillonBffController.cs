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
}