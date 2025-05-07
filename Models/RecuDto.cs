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
        [JsonPropertyName("operateurmobile")]
        public string? operateurmobile { get; set; }

        //reception virement
        [JsonPropertyName("referenceOV")]
        public string? referenceOV { get; set; }
        [JsonPropertyName("banque")]
        public string? banque { get; set; }

        //attribut commun
        [JsonPropertyName("montantApayer")]
        public double montantApayer { get; set; }
        [JsonPropertyName("referenceEtatDecompte")]
        public string? referenceEtatDecompte { get; set; }
        [JsonPropertyName("id_paiement")]
        public int id_paiement { get; set; }

        //attribut de recuperation dashboard 
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("nombrePaiement")]
        public int NombrePaiement { get; set; }
        [JsonPropertyName("montantTotal")]
        public double MontantTotal { get; set; }

        //attribut de confirmation
        [JsonPropertyName("email")]
        public string? email { get; set; }
        [JsonPropertyName("telephone")]
        public string? telephone {get; set;}
    }
}
