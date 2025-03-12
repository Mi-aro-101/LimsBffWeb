using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/prestation")]
public class PrestationController : Controller
{
    private readonly string _prestationUrlService = "http://localhost:5013/api/prestation";
    private readonly HttpClient _httpClient;

    public PrestationController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetPrestations()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_prestationUrlService);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Prestations");
        }
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetPrestation(int id)
    {
        string requestUri = $"{_prestationUrlService}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération de donnée : Prestation");
        }
        return Ok(apiResponse);
    }

    [HttpPost("etat/decompte/{id}")]
    public async Task<ActionResult<ApiResponse>> EtatDeDecompteToPdf(int id)
    {
        string fullUrl = _prestationUrlService+$"/etat/decompte/{id}";
        var response = await _httpClient.PostAsJsonAsync(fullUrl, id);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Error generating PDF");

        var stream = await response.Content.ReadAsStreamAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/pdf";
        var fileName = response.Content.Headers.ContentDisposition?.FileName ?? $"EtatDeDecompte_{id}.pdf";

        return File(stream, contentType, fileName);
    }
}