using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.Immobilisation;

public class MarqueDto
{
    [JsonPropertyName("idMarque")]
    public int IdMarque { get; set; }

    [JsonPropertyName("designation")]
    public required string Designation { get; set; }
}