using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class VDetailsEchantillonDto
{
    [JsonPropertyName("idTravail")]
    public int IdTravail { get; set; }
    [JsonPropertyName("idEchantillon")]
    public int IdEchantillon { get; set; }
    [JsonPropertyName("travaux")]
    public required string Travaux { get; set; }
    [JsonPropertyName("avancee")]
    public required string Avancee { get; set; }
}