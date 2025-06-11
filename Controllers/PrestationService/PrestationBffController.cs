using System.Text.Json;
using System.Threading.Tasks;
using LimsBffWeb.Models;
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

    [HttpPut("transmission/{idPrestation}")]
    public async Task<ActionResult<ApiResponse>> TransmissionPrestation(int idPrestation)
    {
        string requestUri = $"{_prestationUrlService}/transmission/{idPrestation}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, idPrestation);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            return Ok(apiResponse);
        }
        else return BadRequest("Une erreur s'est produite lors de la transmission");
    }

    [HttpPut("livraison/{idPrestation}")]
    public async Task<ActionResult<ApiResponse>> LivraisonPrestation(int idPrestation)
    {
        string requestUri = $"{_prestationUrlService}/livraison/{idPrestation}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, idPrestation);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            return Ok(apiResponse);
        }
        else return BadRequest("Une erreur s'est produite lors de la livraison");
    }

    [HttpPost]
    public ActionResult<ApiResponse> CreatePrestation(CreatePrestationDto prestationDto)
    {   
        Console.WriteLine("kkkk:" + JsonSerializer.Serialize(prestationDto));
        var response =  _httpClient.PostAsJsonAsync(_prestationUrlService, prestationDto).GetAwaiter().GetResult();
        using var responseStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
        ApiResponse? apiResponse = JsonSerializer.DeserializeAsync<ApiResponse>(responseStream).GetAwaiter().GetResult();
        return Ok(apiResponse);
    }

    [HttpPost("tri")]
    public async Task<ActionResult<ApiResponse>> GetPrestations(SortPrestationDto? sorter = null)
    {
        var response =  _httpClient.PostAsJsonAsync(_prestationUrlService+"/tri", sorter).GetAwaiter().GetResult();
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetPrestation(int id)
    {
        string requestUri = $"{_prestationUrlService}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
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

    [HttpPost("fiche/travail/{id}")]
    public async Task<ActionResult<ApiResponse>> FicheTravailPdf(int id)
    {
        string fullUrl = _prestationUrlService+$"/fiche/travail/{id}";
        var response = await _httpClient.PostAsJsonAsync(fullUrl, id);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Error generating PDF");

        var stream = await response.Content.ReadAsStreamAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/pdf";
        var fileName = response.Content.Headers.ContentDisposition?.FileName ?? $"FicheTravail_{id}.pdf";

        return File(stream, contentType, fileName);
    }

    
}