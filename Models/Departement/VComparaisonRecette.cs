using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class VComparaisonRecetteDto
{
    [JsonPropertyName("annee")]
    public int Annee { get; set; }
    [JsonPropertyName("idDepartement")]
    public int IdDepartement { get; set; }
    [JsonPropertyName("designation")]
    public string Designation { get; set; } = string.Empty;
    [JsonPropertyName("chiffreAffaire")]
    public decimal ChiffreAffaire { get; set; }
    [JsonPropertyName("prevision")]
    public decimal Prevision { get; set; }
}