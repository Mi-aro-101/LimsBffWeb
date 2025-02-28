using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models;
using System.Text.Json;
using LimsUtils.Api;

namespace LimsBffWeb.Controllers;
[ApiController]
[Route("/api/departement")]
public class DepartementBffController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _departementServiceUrl = "http://localhost:5000/api/Departement";


    public DepartementBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    [HttpGet]
    public async Task<ActionResult> GetDepartement(int position, int pageSize)
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_departementServiceUrl+$"?position={position}&pageSize={pageSize}");
        if(apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Departements");
        }
        apiResponse.HandleResponse<List<DepartementDto>>();
        
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetDepartementDetails(int id)
    {
        string requestUri = $"{_departementServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération de donnée : Departement");
        }
        apiResponse.HandleResponse<DepartementDto>();
        
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
        else return BadRequest("Data is null");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateDepartement(int id, DepartementDto departementDto)
    {
        string requestUri = $"{_departementServiceUrl}/{id}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, departementDto);
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
        
        return await GetDepartement(1, 2);
    }

    [HttpGet]
    [Route("/api/departement/all")]
    public async Task<ActionResult<ApiResponse>> GetAllDepartements()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_departementServiceUrl+"/all");
        if (apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération de donnée : Departement");
        } 
        apiResponse.HandleResponse<List<DepartementDto>>();
        
        return Ok(apiResponse);
    }

}
