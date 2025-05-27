using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class DelaiDto
    {
	    //attribut client
        [JsonPropertyName("nomClient")]
        public string? nomClient { get; set; }

        //attribut prestation
        [JsonPropertyName("montant")]
        public double montant { get; set; }

        //attribut Etat decompte
        [JsonPropertyName("id_etat_decompte")]
        public int id_etat_decompte { get; set; }

        [JsonPropertyName("referenceEtatDecompte")]
        public string referenceEtatDecompte { get; set; } = string.Empty;

        [JsonPropertyName("date_etat_decompte")]
        public DateTime date_etat_decompte { get; set; }

        //attribut paiement
        [JsonPropertyName("idPaiement")]
        public int idPaiement { get; set; }

        [JsonPropertyName("datePaiement")]
        public DateTime? datePaiement { get; set; }

        [JsonPropertyName("etatPaiement")]
        public bool EtatPaiement { get; set; }

        [JsonPropertyName("id_modePaiementTable")]
        public int id_modePaiementTable { get; set; }

        //attribut delai
        [JsonPropertyName("dateFinDelai")]
        public DateTime DateFinDelai { get; set; }

        [JsonPropertyName("id_modePaiement")]
        public int id_modePaiement { get; set; }

        // attribut supplementaire
        [JsonPropertyName("nombreEchantillon")]
        public int nombreEchantillon { get; set; }
    }
}
