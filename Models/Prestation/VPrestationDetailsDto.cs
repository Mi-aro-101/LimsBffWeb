using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class VPrestationDetailsDto
{
    [JsonPropertyName("idTravail")]
    public int IdTravail { get; set; }
    [JsonPropertyName("idPrestation")]
    public int IdPrestation { get; set; }
    public PrestationDto? Prestation { get; set; }
    [JsonPropertyName("referenceClient")]
    public required string ReferenceClient { get; set; }
    [JsonPropertyName("reference")]
    public required string Reference { get; set; }
    [JsonPropertyName("referenceFicheTravail")]
    public required string ReferenceFicheTravail { get; set; }
    [JsonPropertyName("designation")]
    public required string Designation { get; set; }
    [JsonPropertyName("tarif")]
    public required decimal Tarif { get; set; }
    [JsonPropertyName("idEchantillon")]
    public int IdEchantillon { get; set; }
}