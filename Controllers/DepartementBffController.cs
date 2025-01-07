using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DepartementBff.Models;
using System.Linq;
using Departement.Utils;
using System.Text.Json;

namespace LimsBffWeb.Controllers;
[ApiController]
[Route("/api/departement")]
public class DepartementBffController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _departementServiceUrl = "http://localhost:5189/api/Departement"; // Replace with your ProductService URL


    public DepartementBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    [HttpGet]
    public async Task<ActionResult> GetDepartement()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_departementServiceUrl);
        apiResponse.HandleResponse<List<DepartementDto>>();
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetDepartementDetails(int id)
    {
        string requestUri = $"{_departementServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        apiResponse.HandleResponse<DepartementDto>();
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateDepartement(DepartementDto departementDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_departementServiceUrl, departementDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<DepartementDto>();
            DepartementDto departement = (DepartementDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDepartement(int id)
    {
        string requestUri = $"{_departementServiceUrl}/{id}";
        HttpResponseMessage response = await _httpClient.DeleteAsync(requestUri);
        response.EnsureSuccessStatusCode();
        
        return await GetDepartement();
    }


}
