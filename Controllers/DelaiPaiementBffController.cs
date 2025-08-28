using LimsUtils.Api;
using System.Text.Json;
using LimsBffWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/delai")]
    public class DelaiPaiementBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _delaiURL = "http://localhost:5290/api/delaipaiement";

        public DelaiPaiementBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("ListeAllDelai")]
        public async Task<ActionResult> GetAllListeDelaiAsync()
        {
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_delaiURL);
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<List<DelaiDto>>();
            return Ok(apiresponse);
        }

        [HttpGet("VerificationDelai/{id_etat_decompte}")]
        public async Task<ActionResult> GetVerificationDelaiAccorder(int id_etat_decompte)
        {
            string requestUri = $"{_delaiURL}/DelaiAccorder/{id_etat_decompte}";
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            if (apiresponse == null) return NotFound();

            var message = apiresponse.Message;

            switch (message)
            {
                case "Délai accordé pour le test de période de 6 mois":
                case "Délai accordé pour le test de période de 1 an":
                   {
                        // Convertir le champ "data" en liste de DelaiDto
                        var jsonData = JsonSerializer.Serialize(apiresponse.Data);
                        var listeDelai = JsonSerializer.Deserialize<List<DelaiDto>>(jsonData);

                            //Console.WriteLine(apiresponse.ViewBag["id_etat_decompte"]);
                            
                        // Créer un nouveau ApiResponse enrichi avec le viewBag
                        var reponseAvecViewBag = new ApiResponse
                        {
                            Data = listeDelai,
                            ViewBag = new Dictionary<string, object>
                            {
                                {"id_etat_decompte", apiresponse.ViewBag?["id_etat_decompte"] ?? 0 },
                                {"totalEchantillon", apiresponse.ViewBag?["totalEchantillon"] ?? 0 }                                
                            },
                            Message = apiresponse.Message,
                            IsSuccess = true,
                            StatusCode = 200
                        };

                        return Ok(reponseAvecViewBag);
                    }
                    // break;

                case "Client sous contrat":
                    apiresponse.HandleResponse<SousContratDto>();
                    /*Console.WriteLine($"Message reçu : {apiresponse?.Message}");
                    Console.WriteLine($"Data brute : {JsonSerializer.Serialize(apiresponse?.Data)}");*/
                    break;

                default:
                    // Si d'autres cas doivent être gérés plus tard
                    return BadRequest("Message non reconnu");
            }
            Console.WriteLine($"Data brute : {JsonSerializer.Serialize(apiresponse?.Data)}");
            return Ok(apiresponse);
        }

        [HttpGet("ListeDelaiApayer")]
        public async Task<ActionResult> GetListeDelaiApayerAsync()
        {
            Console.WriteLine(_delaiURL+"/DelaiApayer");
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_delaiURL + "/DelaiApayer");
            if (apiresponse == null) return Ok(new ApiResponse { IsSuccess = false, Message = "Pas de données" });
            apiresponse.HandleResponse<List<DelaiDto>>();
            return Ok(apiresponse);
        } 

        [HttpPost]
        public async Task<ActionResult> AddDelaiPaiementAsync(DelaiDto delai)
        {
            var response = await _httpClient.PostAsJsonAsync(_delaiURL, delai);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
                apiResponse.HandleResponse<DelaiDto>();
                DelaiDto departement = (DelaiDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }

        [HttpPut("PaiementDirect/{id_etat_decompte}/{modepaiement}")]
        public async Task<ActionResult> UpdateDelaiPaiementAsync(int id_etat_decompte, string modepaiement)
        {
            var requestUri = $"{_delaiURL}/PaiementDirect/{id_etat_decompte}/{modepaiement}";

            // var response = await _httpClient.PutAsJsonAsync<ApiResponse>(requestUri, null); // tu peux aussi utiliser PutAsync si pas besoin d'envoyer de données
            var response = await _httpClient.PutAsync(requestUri, null); // tu peux aussi utiliser PutAsync si pas besoin d'envoyer de données
            if (!response.IsSuccessStatusCode)
                return BadRequest("Erreur lors de l'appel de l'API externe.");

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);

            if (apiResponse?.IsSuccess == true)
            {
                return Ok(apiResponse);
            }
            else
            {
                return BadRequest("Ohatran'ny nisy olana tao a");
            }
        }

        [HttpPut("PaiementParChangement/{id_etat_decompte}/{modepaiement}")]
        public async Task<ActionResult> UpdateDelaiPaiementParChangementAsync(int id_etat_decompte, string modepaiement)
        {
            var requestUri = $"{_delaiURL}/PaiementParChangement/{id_etat_decompte}/{modepaiement}";

            // var response = await _httpClient.PutAsJsonAsync<ApiResponse>(requestUri, null); // tu peux aussi utiliser PutAsync si pas besoin d'envoyer de données
            var response = await _httpClient.PutAsync(requestUri, null); // tu peux aussi utiliser PutAsync si pas besoin d'envoyer de données
            if (!response.IsSuccessStatusCode)
                return BadRequest("Erreur lors de l'appel de l'API externe.");

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);

            if (apiResponse?.IsSuccess == true)
            {
                return Ok(apiResponse);
            }
            else
            {
                return BadRequest("Ohatran'ny nisy olana tao a");
            }
        }

        [HttpGet("DelaiPaiementEnAttente")]
        public async Task<ActionResult> GetDelaiPaiementEnAttenteAsync()
        {
            var requestUri = $"{_delaiURL}/DelaiEnAttente";
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            if (apiresponse == null) return Ok(new ApiResponse { IsSuccess = false, Message = "Pas de données" });
            apiresponse.HandleResponse<List<DashboardDelaiDto>>();
            return Ok(apiresponse);
        }

        [HttpGet("Prestations")]
        public async Task<ActionResult> GetPrestaionsAsync()
        {
            var requestUri = $"{_delaiURL}/PrestationAPayer";
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            if (apiresponse == null) return Ok(new ApiResponse { IsSuccess = false, Message = "Pas de données" });
            apiresponse.HandleResponse<List<PaiementDto>>();
            return Ok(apiresponse);
        }

    }
}
