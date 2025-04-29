using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.Immobilisation
{
    public class EntreeImmobilisationDto
    {
        [JsonPropertyName("idEntreeImmobilisation")]
        public int IdEntreeImmobilisation { get; set; }

        [JsonPropertyName("quantite")]
        public int? Quantite { get; set; }

        [JsonPropertyName("prixAchat")]
        public decimal? PrixAchat { get; set; }

        [JsonPropertyName("dateEntree")]
        public DateTime? DateEntree { get; set; }

        [JsonPropertyName("bonReception")]
        public string? BonReception { get; set; }

        [JsonPropertyName("bonDeCommande")]
        public string? BonDeCommande { get; set; }

        [JsonPropertyName("numeroFacture")]
        public string? NumeroFacture { get; set; }

        [JsonPropertyName("idFournisseur")]
        public int IdFournisseur { get; set; }

        [JsonPropertyName("fournisseur")]
        public FournisseurDto? Fournisseur { get; set; }

        [JsonPropertyName("idImmobilisation")]
        public int? IdImmobilisation { get; set; }

        [JsonPropertyName("immobilisation")]
        public ImmobilisationDto? Immobilisation { get; set; }
    }
}
