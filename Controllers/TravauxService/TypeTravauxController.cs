using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models;
using System.Text.Json;
using LimsUtils.Api;
using LimsFrontEnd.Utils;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/type/travaux")]
public class TypeTravauxBffController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _typetravauxServiceUrl = "http://localhost:5126/api/type/travaux"; // Replace with your ProductService URL

    public TypeTravauxBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<ActionResult> GetTypeTravauxFrom(int position, int pageSize)
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_typetravauxServiceUrl+$"?position={position}&pageSize={pageSize}");
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Type Travaux");
        }
        apiResponse.HandleResponse<List<TypeTravauxDto>>();
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTypeTravauxDetails(int id)
    {
        string requestUri = $"{_typetravauxServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest($"Une erreur s'est produite lors de la récupération de donnée : Travaux {id}");
        }
        apiResponse.HandleResponse<TypeTravauxDto>();
        
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateTypeTravaux([FromBody] TypeTravauxDto typeTravaux)
    {
        var response = await _httpClient.PostAsJsonAsync(_typetravauxServiceUrl, typeTravaux);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest($"Une erreur s'est produite lors de la création des données : Type travaux {typeTravaux.Designation}");
        }
        else if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<TypeTravauxDto>();
            TypeTravauxDto typeTravauxDto = (TypeTravauxDto)apiResponse.Data;
        }
        return Ok(apiResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> EditTypeTravaux(int id, TypeTravauxDto typeTravauxDto)
    {
        string requestUri = $"{_typetravauxServiceUrl}/{id}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, typeTravauxDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest($"Une erreur s'est produite lors de la mis à jour de donnée : Type Travaux {typeTravauxDto.Designation}");
        }
        else if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<TypeTravauxDto>();
            TypeTravauxDto typeTravaux = (TypeTravauxDto)apiResponse.Data;
        }
        return Ok(apiResponse);
    }

    [HttpPost("tarifier/{id}")]
    public async Task<ActionResult> Tarifier(int id, [FromBody] FormuleDto formuleDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_typetravauxServiceUrl+$"/tarifier/{id}", formuleDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors du calcul de tarification");
        }
        else if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<FormuleDto>();
            FormuleDto formule = (FormuleDto)apiResponse.Data;
        }
        return Ok(apiResponse);
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse>> GetTotalTypeTravaux()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_typetravauxServiceUrl+"/all");
        if (apiResponse?.IsSuccess == false || apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    
}