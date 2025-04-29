using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.Outillage
{
    public class FournisseurOutillageDto
    {
        [JsonPropertyName("idFournisseur")]
        public int IdFournisseur { get; set; }

        [JsonPropertyName("designation")]
        public  string? Designation { get; set; }
    }
}
