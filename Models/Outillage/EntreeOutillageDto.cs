using System.Text.Json.Serialization;
using LimsBffWeb.Models.Immobilisation;

namespace LimsBffWeb.Models.Outillage
{
    public class EntreeOutillageDto
    {
        [JsonPropertyName("idEntreeOutillage")]
        public int IdEntreeOutillage { get; set; }

        [JsonPropertyName("prixAchat")]
        public decimal PrixAchat { get; set; }

        [JsonPropertyName("quantite")]
        public int Quantite { get; set; }

        [JsonPropertyName("bonReception")]
        public string? BonReception { get; set; }

        [JsonPropertyName("dateEntree")]
        public DateTime DateEntree { get; set; }

        [JsonPropertyName("idFournisseur")]
        public int IdFournisseur { get; set; }

        [JsonPropertyName("idOutillage")]
        public int IdOutillage { get; set; }

        [JsonPropertyName("fournisseur")]
        public FournisseurOutillageDto? Fournisseur { get; set; }

        [JsonPropertyName("outillage")]
        public OutillageDto? Outillage { get; set; }
    }
}
