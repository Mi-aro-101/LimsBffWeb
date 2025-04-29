using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.Outillage
{
    public class MarqueOutillageDto
    {
        [JsonPropertyName("idMarque")]
        public int IdMarque { get; set; }

        [JsonPropertyName("designation")]
        public string? Designation { get; set; }
    }
}
