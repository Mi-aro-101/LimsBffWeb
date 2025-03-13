using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class TravauxDto
{
    [JsonPropertyName("idTypeTravaux")]
    public int IdTypeTravaux { get; set; }
    [JsonPropertyName("designation")]
    public string? Designation { get; set; }
}