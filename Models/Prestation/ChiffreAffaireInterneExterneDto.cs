using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class ChiffreAffaireInterneExterneDto
{
    [JsonPropertyName("isInterne")]
    public int? isInterne { get; set; }
    [JsonPropertyName("montant")]
    public decimal? Montant { get; set; }
    [JsonPropertyName("annee")]
    public int? Annee { get; set; } = DateTime.Now.Year;
    [JsonPropertyName("mois")]
    public int? Mois { get; set; } = DateTime.Now.Month;
}