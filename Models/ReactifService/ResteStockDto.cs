using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class ResteStockDto
{
    [JsonPropertyName("quantite")]
    public double Quantite { get; set; }
    [JsonPropertyName("unite")]
    public string Unite { get; set; } = string.Empty;
    [JsonPropertyName("dateParam")]
    public DateTime DateParam { get; set; } = DateTime.Now;
    [JsonPropertyName("idReactif")]
    public int IdReactif { get; set; }
}
