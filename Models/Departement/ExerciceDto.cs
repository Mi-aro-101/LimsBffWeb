using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class ExerciceDto
{
    [JsonPropertyName("idExercice")]
    public int IdExercice { get; set; }
    [JsonPropertyName("dateDebut")]
    public DateOnly DateDebut { get; set; }
    [JsonPropertyName("dateFin")]
    public DateOnly? DateFin { get; set; }
}