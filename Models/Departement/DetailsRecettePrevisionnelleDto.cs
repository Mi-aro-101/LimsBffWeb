using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class DetailsRecettePrevisionnelleDto
{
    [JsonPropertyName("idDetailsRecettePrevisionnelle")]
    public int IdDetailsRecettePrevisionnelle { get; set; }
    [JsonPropertyName("idDepartement")]
    public int IdDepartement { get; set; }
    [JsonPropertyName("departement")]
    public DepartementDto? Departement { get; set; }
    [JsonPropertyName("idRecettePrevisionnelle")]
    public int IdRecettePrevisionnelle { get; set; }
    [JsonPropertyName("recettePrevisionnelle")]
    public RecettePrevisionnelleDto? RecettePrevisionnelle { get; set; }

    [JsonPropertyName("montant")]
    public decimal Montant { get; set; }
}