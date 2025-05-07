using Microsoft.EntityFrameworkCore.Storage.Internal;
using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class PaiementDto
    {
        //attribut affichage paiement espece
        [JsonPropertyName("titre")]
        public string? titre { get; set; }
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
        [JsonPropertyName("ref_contrat")]
        public string? ref_contrat { get; set; }
        [JsonPropertyName("montant")]
        public double montant { get; set; }
        [JsonPropertyName("etatDecompte")]
        public string? etatDecompte { get; set; }

        //attribut paiement espece post
        [JsonPropertyName("datePaiement")]
        public DateTime DatePaiement { get; set; }
        [JsonPropertyName("modePaiement")]
        public int ModePaiement { get; set; }
        [JsonPropertyName("etatPaiement")]
        public int EtatPaiement { get; set; }
        [JsonPropertyName("nomPayant")]
        public string? nomPayant { get; set; }
        [JsonPropertyName("prenomPayant")]
        public string? prenomPayant { get; set; }
        [JsonPropertyName("contactdupayant")]
        public int contactdupayant { get; set; }

        //attribut commun
        [JsonPropertyName("id_etat_decompte")]
        public int id_etat_decompte { get; set; }
    }
}
