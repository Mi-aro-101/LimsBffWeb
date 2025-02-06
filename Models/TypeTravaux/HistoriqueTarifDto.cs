using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class HistoriqueTarifDto
{
    [JsonPropertyName("idHistoriqueTarif")]
    public int IdHistoriqueTarif { get; set; }
    [JsonPropertyName("tarif")]
    public decimal Tarif { get; set; }
    [JsonPropertyName("dateChangement")]
    public DateTime? DateChangement { get; set; }
    [JsonPropertyName("idTypeTravaux")]
    public int IdTypeTravaux { get; set; }
}