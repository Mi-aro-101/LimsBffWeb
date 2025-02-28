using System.Text.Json;
using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/details/recette/previsionnelle")]
public class DetailsRecettePrevisionnelleController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _detailsRecettePrevisionnelleServiceUrl = "http://localhost:5000/api/details/recette/previsionnelle";

    public DetailsRecettePrevisionnelleController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost]
    public async Task<ActionResult> CreateDetailsRecettePrevisionnelle(DetailsRecettePrevisionnelleDto detailsRecettePrevisionnelle)
    {
        var response = await _httpClient.PostAsJsonAsync(_detailsRecettePrevisionnelleServiceUrl, detailsRecettePrevisionnelle);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if(apiResponse?.IsSuccess == false)
        {
            return BadRequest("Une erreur s'est produite lors de la création du details de la recette prévisionnelle");
        }
        if (apiResponse?.Data != null)
        {
            apiResponse.HandleResponse<DetailsRecettePrevisionnelleDto>();
            DetailsRecettePrevisionnelleDto recettePrevisionnelleDto = (DetailsRecettePrevisionnelleDto)apiResponse.Data;
            return RedirectToRoute( new {
                controller = "recettePrevisionnelle",
                action = "GetRecettePrevisionnelle",
                id = detailsRecettePrevisionnelle.IdRecettePrevisionnelle
            });
        }
        else return BadRequest("Data is null");
    }
    
}