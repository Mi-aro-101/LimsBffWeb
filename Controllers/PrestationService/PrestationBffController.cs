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
}