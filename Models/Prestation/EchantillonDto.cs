using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class EchantillonDto
{
    [JsonPropertyName("idEchantillon")]
    public int? IdEchantillon { get; set; }
    [JsonPropertyName("note")]
    public string? Note { get; set; } 
    [JsonPropertyName("provenance")]
    public string? Provenance { get; set; }
    [JsonPropertyName("datePrelevement")]
    public DateOnly? DatePrelevement { get; set; }
    [JsonPropertyName("idTypeEchantillon")]
    public int IdTypeEchantillon { get; set; }
}