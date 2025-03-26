using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class EncaissementJournalierDto
    {      
        [JsonPropertyName("id_etat_decompte")]
        public int id_etat_decompte { get; set; }

        [JsonPropertyName("idEtatJournalier")]
        public int idEtatJournalier { get; set; }
        
        [JsonPropertyName("dateEncaissement")]
        public DateTime? dateEncaissement { get; set; }

        [JsonPropertyName("etatDecompte")]
        public string? EtatDecompte { get; set; }

        [JsonPropertyName("clients")]
        public string? clients { get; set; }

        [JsonPropertyName("montant")]
        public double montant { get; set; }

        [JsonPropertyName("observation")]
        public int observation { get; set; }
    }
}
