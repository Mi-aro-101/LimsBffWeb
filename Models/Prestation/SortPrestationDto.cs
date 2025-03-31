using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class SortPrestationDto
{
    [JsonPropertyName("referenceFicheTravail")]
    public string? ReferenceFicheTravail { get; set; } = string.Empty;
    [JsonPropertyName("idEtatPrestation")]
    public int? IdEtatPrestation { get; set; }
    [JsonPropertyName("anneeExercice")]
    public int? AnneeExercice { get; set; }
}