using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models;
using LimsBffWeb.Utils;
using System.Text.Json;

namespace LimsBffWeb.Controllers;
[ApiController]
[Route("/api/role")]
public class RoleBffController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _roleServiceUrl = "http://localhost:5042/api/role"; // Replace with your ProductService URL


    public RoleBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    [Route("/api/role/total")]
    public async Task<ActionResult<ApiResponse>> GetTotalRoles()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_roleServiceUrl+"/all");
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetRoles(int position, int pageSize)
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_roleServiceUrl+$"?position={position}&pageSize={pageSize}");
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetRole(int id)
    {
        string requestUri = $"{_roleServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse == null) return NotFound();
        
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateRole(RoleDto roleDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_roleServiceUrl, roleDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<RoleDto>();
            RoleDto role = (RoleDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRole(int id, RoleDto roleDto)
    {
        string requestUri = $"{_roleServiceUrl}/{id}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, roleDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null)
        {
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }

    //On ne doit pas supprimer un role

    // [HttpPut("delete/{id}")]
    // public async Task<ActionResult<ApiResponse>> DeleteRole(int id, RoleDto role)
    // {
    //     Console.WriteLine(JsonSerializer.Serialize(role));
    //     string requestUri = $"{_roleServiceUrl}/{id}";
    //     var response = await _httpClient.DeleteAsync(requestUri);
        
    //     return NoContent();
    // }
}
