using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

// Modification 1, 2, 3

public class ChiffreAffaireDto
{
    [JsonPropertyName("annee")]
    public int? Annee { get; set;} = DateTime.Now.Year;
    [JsonPropertyName("mois")]
    public int? Mois { get; set;} = DateTime.Now.Month;
    [JsonPropertyName("jour")]
    public int? Jour { get; set; }
    [JsonPropertyName("montant")]
    public decimal? Montant { get; set; }
}