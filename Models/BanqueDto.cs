using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class BanqueDto
    {
        [JsonPropertyName("id_banque")]
        public int id_banque { get; set; }
        [JsonPropertyName("designation")]
        public string? designation { get; set; }
    }
}