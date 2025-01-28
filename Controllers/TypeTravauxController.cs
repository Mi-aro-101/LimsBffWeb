using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models;
using System.Linq;
using LimsBffWeb.Utils;
using System.Text.Json;

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
        apiResponse.HandleResponse<List<TypeTravauxDto>>();
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTypeTravauxDetails(int id)
    {
        string requestUri = $"{_typetravauxServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        apiResponse.HandleResponse<TypeTravauxDto>();
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateTypeTravaux([FromBody] TypeTravauxDto typeTravaux)
    {
        var response = await _httpClient.PostAsJsonAsync(_typetravauxServiceUrl, typeTravaux);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<TypeTravauxDto>();
            TypeTravauxDto typeTravauxDto = (TypeTravauxDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> EditTypeTravaux(int id, TypeTravauxDto typeTravauxDto)
    {
        string requestUri = $"{_typetravauxServiceUrl}/{id}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, typeTravauxDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<TypeTravauxDto>();
            TypeTravauxDto typeTravaux = (TypeTravauxDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }
}