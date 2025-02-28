using System.Text.Json.Serialization;

namespace LimsFrontEnd.Utils;

public class FormuleDto
{
    [JsonPropertyName("variable")]
    public Dictionary<string, decimal> Variable { get; set; } = new Dictionary<string, decimal>();
    [JsonPropertyName("tarif")]
    public Double? Tarif { get; set; }
    [JsonPropertyName("formuleString")]
    public string? formuleString { get; set; }
    [JsonPropertyName("formuleChosed")]
    public string? FormuleChosed { get; set; }
}