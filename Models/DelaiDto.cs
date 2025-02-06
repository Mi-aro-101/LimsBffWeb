using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class DelaiDto
    {
        //attribut Etat decompte
        [JsonPropertyName("")]
        public int id_etat_decompte { get; set; }
        [JsonPropertyName("")]
        public string referenceEtatDecompte { get; set; } = string.Empty;
        [JsonPropertyName("")]
        public string date_etat_decompte { get; set; } = string.Empty;

        //attribut paiement
        [JsonPropertyName("")]
        public int idPaiement { get; set; }
        [JsonPropertyName("")]
        public DateTime? datePaiement { get; set; }
        [JsonPropertyName("")]
        public int EtatPaiement { get; set; }

        //attribut delai
        [JsonPropertyName("")]
        public DateTime DateFinDelai { get; set; }
        [JsonPropertyName("")]
        public int modePaiement { get; set; }
    }
}
