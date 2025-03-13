using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.ReactifService
{
    public class UniteDto
    {
        [JsonPropertyName("idUnite")]
        public int IdUnite { get; set; }

        [JsonPropertyName("designation")]
        public string? Designation { get; set; }
    }
}
