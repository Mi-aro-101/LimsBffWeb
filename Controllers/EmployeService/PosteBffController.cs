using Microsoft.AspNetCore.Mvc;
using LimsBffWeb.Models;
using System.Text.Json;
using LimsUtils.Api;

namespace LimsBffWeb.Controllers;
[ApiController]
[Route("/api/poste")]
public class PosteBffController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _posteServiceUrl = "http://localhost:5201/api/Poste"; // Replace with your ProductService URL


    public PosteBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    [HttpGet]
    public async Task<ActionResult> GetPoste(int position, int pageSize)
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_posteServiceUrl+$"?position={position}&pageSize={pageSize}");
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Poste");
        }
        apiResponse.HandleResponse<List<PosteDto>>();
        
        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPosteDetails(int id)
    {
        string requestUri = $"{_posteServiceUrl}/{id}";
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Employes");
        }
        apiResponse.HandleResponse<PosteDto>();
        
        return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePoste(PosteDto posteDto)
    {

        // var role = HttpContext.Items["Role"]?.ToString();

        // if (role != "Admin")
        // {
        //     return Forbid();
        // }

        var response = await _httpClient.PostAsJsonAsync(_posteServiceUrl, posteDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Employes");
        }
        else if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<PosteDto>();
            PosteDto poste = (PosteDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePoste(int id,int pageSize)
    {
        string requestUri = $"{_posteServiceUrl}/{id}";
        HttpResponseMessage response = await _httpClient.DeleteAsync(requestUri);
        response.EnsureSuccessStatusCode();
        
        return await GetPoste(0,pageSize);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> EditPoste(int? id, PosteDto posteDto)
    {
        string requestUri = $"{_posteServiceUrl}/{id}";
        var response = await _httpClient.PutAsJsonAsync(requestUri, posteDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la mis à jour des données : Employes");
        }
        else if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<PosteDto>();
            PosteDto poste = (PosteDto)apiResponse.Data;
            return Ok(apiResponse);
        }
        else return BadRequest("Ohatran'ny nisy olana tao a");
    }

    [HttpGet]
    [Route("/api/poste/all")]
    public async Task<ActionResult<ApiResponse>> GetAllPostes()
    {
        ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_posteServiceUrl+"/all");
        if (apiResponse?.IsSuccess == false || apiResponse == null)
        {
            return BadRequest("Une erreur s'est produite lors de la récupération des données : Postes");
        }
        apiResponse.HandleResponse<List<PosteDto>>();
        
        return Ok(apiResponse);
    }
}
