using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models;
using LimsBffWeb.Utils;
using System.Text.Json;

namespace LimsBffWeb.Controllers;
[ApiController]
[Route("/api/employe")]
public class EmployeBffController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _employeServiceUrl = "http://localhost:5201/api/employe"; // Replace with your ProductService URL


    public EmployeBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    [Route("/api/employe/total")]
    public async Task<ActionResult<ApiResponse>> GetTotalEmployes()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_employeServiceUrl+"/total");
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetEmployes(int position, int pageSize)
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_employeServiceUrl+$"?position={position}&pageSize={pageSize}");
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetEmploye(int id)
    {
        string requestUri = $"{_employeServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateEmploye(EmployeDto employeDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_employeServiceUrl, employeDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<EmployeDto>();
            EmployeDto employe = (EmployeDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEmploye(int id, EmployeDto employeDto)
    {
        string requestUri = $"{_employeServiceUrl}/{id}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, employeDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteEmploye(int id)
    {
        string requestUri = $"{_employeServiceUrl}/{id}";
        HttpResponseMessage response = await _httpClient.DeleteAsync(requestUri);
        
        return await GetEmployes(1, 2);
    }
}
