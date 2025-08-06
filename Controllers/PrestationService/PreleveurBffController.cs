using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/preleveur")]
public class PreleveurBffCController : Controller
{
    private readonly string _clientServiceUrl = "http://localhost:5013/api/preleveur";
    private readonly HttpClient _httpClient;

    public PreleveurBffCController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetPreleveurs()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_clientServiceUrl);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Preleveurs");
        }
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPreleveur(int id)
    {
        string requestUri = $"{_clientServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest($"Une erreur s'est produite lors de la récupération de donnée : Preleveur {id}");
        }
        return Ok(apiResponse);
    }
}