using LimsBffWeb.Models;
using LimsUtils.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimsBffWeb.Controllers
{
    [ApiController]
    [Route("api/notedebit")]
    public class DemandeNoteDebitBffController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _demandeNoteDebitURL = "http://localhost:5290/api/demandenotedebit";

        public DemandeNoteDebitBffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<ActionResult> AddDemandeNodeDebit(DemandeDto noteDebit)
        {
            var response = await _httpClient.PostAsJsonAsync(_demandeNoteDebitURL, noteDebit);

            using var responseStream = await response.Content.ReadAsStreamAsync();
            ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
            if (apiResponse?.Data != null)
            {
               
                JsonElement data = (JsonElement)apiResponse.Data;

                if (data.TryGetProperty("demande", out var demandeElement) && data.TryGetProperty("pdfBase64", out var pdfBase64Element))
                {
                    var pdfBase64List = JsonSerializer.Deserialize<List<string>>(pdfBase64Element.GetRawText());

                    // 👇 Ici tu peux afficher ou utiliser les PDF en base64
                    // foreach (var base64 in pdfBase64List)
                    // {
                    //     Console.WriteLine("PDF en base64 reçu : " + base64);
                    // }

                    DemandeDto? demande = JsonSerializer.Deserialize<DemandeDto>(demandeElement.GetRawText());
                    if (demande == null)
                    {
                        return BadRequest("La désérialisation de la demande a échoué.");
                    }
                    return Ok(apiResponse); 
                }                
                else
                {
                    return BadRequest("Aucune propriété attendue trouvée dans la réponse.");
                }           
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }

        [HttpGet("Demande_A_Faire")]
        public async Task<ActionResult> GetNoteDebit()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_demandeNoteDebitURL+ "/NoteDoitfaire");
            if (apiResponse == null) return NotFound();
            apiResponse.HandleResponse<List<DemandeDto>>();
            return Ok(apiResponse);
        }

        [HttpGet("{id_etat_decompte}")]
        public async Task<ActionResult> GetInfoDemande(int id_etat_decompte)
        {
            string requestUri = $"{_demandeNoteDebitURL}/{id_etat_decompte}";
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            if (apiresponse == null) return NotFound();
            apiresponse.HandleResponse<List<DemandeDto>>();
            return Ok(apiresponse);
        }

        [HttpGet("AllListe")]
        public async Task<ActionResult> GetListNoteDebit()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_demandeNoteDebitURL+"/Liste");
            if( apiResponse == null ) return NotFound();
            apiResponse.HandleResponse<List<DemandeDto>>();
            return Ok(apiResponse);
        }

        [HttpGet("DemandeOublie")]
        public async Task<ActionResult> GetVerificationDemandeOublie()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_demandeNoteDebitURL + "/Verification");
            if (apiResponse == null) return NotFound();
            apiResponse.HandleResponse<List<DemandeDto>>();
            return Ok(apiResponse);
        }
    }
}
