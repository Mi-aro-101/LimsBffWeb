using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class RecuDto
    {
        //reception espece
        [JsonPropertyName("montantRecu")]
        public double montantRecu { get; set; }

        //reception mobile 
        [JsonPropertyName("referenceMobile")]
        public int referenceMobile { get; set; }

        //reception virement
        [JsonPropertyName("referenceOV")]
        public string? referenceOV { get; set; }

        //attribut commun
        [JsonPropertyName("referenceEtatDecompte")]
        public string? referenceEtatDecompte { get; set; }
        [JsonPropertyName("id_paiement")]
        public int id_paiement { get; set; }
        [JsonPropertyName("email")]
        public string? email { get; set; }
    }
}
