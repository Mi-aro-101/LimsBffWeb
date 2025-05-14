using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.Immobilisation;

public class EmployeImmobilisationDto
{
    [JsonPropertyName("idEmploye")]
    public int IdEmploye { get; set; }

    [JsonPropertyName("matricule")]
    public required string Matricule { get; set; }

    [JsonPropertyName("nom")]
    public required string Nom { get; set; }

    [JsonPropertyName("prenom")]
    public required string Prenom { get; set; }

   

    [JsonPropertyName("cin")]
    public required string Cin { get; set; }

    [JsonPropertyName("contact")]
    public required string Contact { get; set; }

    [JsonPropertyName("adresse")]
    public required string Adresse { get; set; }

    [JsonPropertyName("manager")]
    public string? Manager { get; set; } = string.Empty;

    [JsonPropertyName("statut")]
    public int Statut { get; set; }
}