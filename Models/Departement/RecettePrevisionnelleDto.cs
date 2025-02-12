using System.Text.Json.Serialization;
using LimsBffWeb.Models;

namespace LimsBffWeb.Models;

public class RecettePrevisionnelleDto
{
    [JsonPropertyName("idRecettePrevisionnelle")]
    public int IdRecettePrevisionnelle { get; set; }
    [JsonPropertyName("dateRecettePrevisionnelle")]
    public DateOnly DateRecettePrevisionnelle { get; set; }
    [JsonPropertyName("idExercice")]
    public int IdExercice { get; set; }
    [JsonPropertyName("exercice")]
    public ExerciceDto? Exercice { get; set; }
    [JsonPropertyName("montantTotal")]
    public decimal MontantTotal { get; set; }
    [JsonPropertyName("detailsRecettePrevisionnelles")]
    public ICollection<DetailsRecettePrevisionnelleDto> DetailsRecettePrevisionnelles { get; set;} = new List<DetailsRecettePrevisionnelleDto>();
}