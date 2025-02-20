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
                apiResponse.HandleResponse<DepartementDto>();
                DepartementDto departement = (DepartementDto)apiResponse.Data;
                return Ok(apiResponse);
            }
            else return BadRequest("Ohatran'ny nisy olana tao a");
        }

        /*[HttpGet]
        public async Task<ActionResult> GetNoteDebit(int position, int pageSize)
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_demandeNoteDebitURL+$"?position={position}&pageSize={pageSize}");
            apiResponse.HandleResponse<List<DemandeDto>>();
            if( apiResponse == null ) return NotFound();
            return Ok(apiResponse);
        }*/

        [HttpGet("Demande_A_Faire")]
        public async Task<ActionResult> GetNoteDebit()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_demandeNoteDebitURL+ "/NoteDoitfaire");
            apiResponse.HandleResponse<List<DemandeDto>>();
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }

        [HttpGet("{id_etat_decompte}")]
        public async Task<ActionResult> GetInfoDemande(int id_etat_decompte)
        {
            string requestUri = $"{_demandeNoteDebitURL}/{id_etat_decompte}";
            ApiResponse? apiresponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri);
            apiresponse.HandleResponse<List<DemandeDto>>();
            if (apiresponse == null) return NotFound();
            return Ok(apiresponse);
        }

        [HttpGet("AllListe")]
        public async Task<ActionResult> GetListNoteDebit()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_demandeNoteDebitURL+"/Liste");
            apiResponse.HandleResponse<List<DemandeDto>>();
            if( apiResponse == null ) return NotFound();
            return Ok(apiResponse);
        }

        [HttpGet("DemandeOublie")]
        public async Task<ActionResult> GetVerificationDemandeOublie()
        {
            ApiResponse? apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(_demandeNoteDebitURL + "/Verification");
            apiResponse.HandleResponse<List<DemandeDto>>();
            if (apiResponse == null) return NotFound();
            return Ok(apiResponse);
        }
    }
}
