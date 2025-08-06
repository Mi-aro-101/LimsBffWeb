using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class PreleveurDto
{
    [JsonPropertyName("idPreleveur")]
    public int? IdPreleveur { get; set; }
    [JsonPropertyName("designation")]
    public string? Designation { get; set; }
}