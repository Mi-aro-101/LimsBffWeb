using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class ChiffreAffaireDto
{
    [JsonPropertyName("annee")]
    public int? Annee { get; set;}
    [JsonPropertyName("mois")]
    public int? Mois { get; set;}
    [JsonPropertyName("jour")]
    public int? Jour { get; set; }
    [JsonPropertyName("montant")]
    public decimal? Montant { get; set; }
}