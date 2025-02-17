using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models;
using System.Text.Json;
using LimsUtils.Api;

namespace LimsBffWeb.Controllers;
[ApiController]
[Route("/api/type/echantillon")]
public class TypeEchantillonBffController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _typeEchantillonServiceUrl = "http://localhost:5013/api/type/echantillon"; // Replace with your PrestationService URL


    public TypeEchantillonBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    [Route("/api/type/echantillon/all")]
    public async Task<ActionResult<ApiResponse>> GetTotalTypeEchantillons()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_typeEchantillonServiceUrl+"/all");
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetTypeEchantillons(int position, int pageSize)
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_typeEchantillonServiceUrl+$"?position={position}&pageSize={pageSize}");
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTypeEchantillon(int id)
    {
        string requestUri = $"{_typeEchantillonServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateTypeEchantillon(TypeEchantillonDto typeEchantillonDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_typeEchantillonServiceUrl, typeEchantillonDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<TypeEchantillonDto>();
            TypeEchantillonDto typeEchantillon = (TypeEchantillonDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest(apiResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTypeEchantillon(int id, TypeEchantillonDto typeEchantillonDto)
    {
        string requestUri = $"{_typeEchantillonServiceUrl}/{id}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, typeEchantillonDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            return Ok(apiResponse);
        }
        else return BadRequest(apiResponse);
    }

    //On ne doit pas supprimer un typeEchantillon

    // [HttpPut("delete/{id}")]
    // public async Task<ActionResult<ApiResponse>> DeleteTypeEchantillon(int id, TypeEchantillonDto typeEchantillon)
    // {
    //     Console.WriteLine(JsonSerializer.Serialize(typeEchantillon));
    //     string requestUri = $"{_typeEchantillonServiceUrl}/{id}";
    //     var response = await _httpClient.DeleteAsync(requestUri);
        
    //     return NoContent();
    // }
}
