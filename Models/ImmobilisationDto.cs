using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class ImmobilisationDto
{
    [JsonPropertyName("idImmobilisation")]
    public int IdImmobilisation { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("designation")]
    public required string Designation { get; set; }

    [JsonPropertyName("idMarque")]
    public int IdMarque { get; set; }

    [JsonPropertyName("marque")]
    public required MarqueDto Marque { get; set; } // Correspond à la structure de ton API
}