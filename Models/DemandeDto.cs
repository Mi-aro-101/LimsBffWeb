using Microsoft.EntityFrameworkCore.Storage.Internal;
using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class DemandeDto
    {        
        // Attributs de `DemandeDto`
        [JsonPropertyName("clients")]
        public string? clients { get; set; }
        [JsonPropertyName("email")]
        public string? email { get; set; }        
        [JsonPropertyName("adresse")]
        public string? adresse { get; set; }
        [JsonPropertyName("contact")]
        public string? contact { get; set; }
        [JsonPropertyName("identite")]
        public string? identite { get; set; }
        [JsonPropertyName("etatDecompte")]
        public string? etatDecompte { get; set; }
        [JsonPropertyName("datePaiement")]
        public string? datePaiement { get; set; }
        [JsonPropertyName("nombreEchantillon")]
        public int? nombreEchantillon { get; set; }

        // Attributs de `DemandePost`
        [JsonPropertyName("reference")]
        public int? reference { get; set; }
        [JsonPropertyName("dateDemande")]
        public DateTime? dateDemande { get; set; }
        [JsonPropertyName("objet")]
        public string? objet { get; set; }
        [JsonPropertyName("montant_literal")]
        public string? montant_literal { get; set; }
        [JsonPropertyName("travaux")]
        public string? travaux { get; set; }

        // Attributs Etat_decompte utile pour liste
        [JsonPropertyName("referenceEtatDecompte")]
        public string? referenceEtatDecompte { get; set; }
        [JsonPropertyName("date_etat_decompte")]
        public string? dateEtatDecompte { get; set; }

        // Attributs communs
        [JsonPropertyName("montant")]
        public double montant { get; set; }
        [JsonPropertyName("id_etat_decompte")]
        public int idEtatDecompte { get; set; }

    }
}
