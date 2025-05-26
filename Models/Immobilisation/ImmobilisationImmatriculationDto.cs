using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.Immobilisation
{
    public class ImmobilisationImmatriculationDto
    {
        [JsonPropertyName("idImmobilisationPropre")]
        public int IdImmobilisationPropre { get; set; }

        [JsonPropertyName("matricule")]
        public string? Matricule { get; set; }

        [JsonPropertyName("remarque")]
        public string? Remarque { get; set; }

        [JsonPropertyName("etatInitiale")]
        public string? EtatInitiale { get; set; }
        
        [JsonPropertyName("idEntreeImmobilisation")]
        public int? IdEntreeImmobilisation { get; set; }

        [JsonPropertyName("entreeImmobilisation")]
        public EntreeImmobilisationDto? EntreeImmobilisation { get; set; }
    }
}