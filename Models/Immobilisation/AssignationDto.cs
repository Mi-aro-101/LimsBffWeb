using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.Immobilisation;

public class AssignationDto
{
    [JsonPropertyName("idAssignation")]
    public int? IdAssignation { get; set; } // Changé de int à int? pour permettre null

    [JsonPropertyName("dateAssignation")]
        public DateTime DateAssignation { get; set; } // Modifié en nullable

    [JsonPropertyName("idLocalisation")]
    public int? IdLocalisation { get; set; }

    [JsonPropertyName("idImmobilisationPropre")]
    public int IdImmobilisationPropre { get; set; }

    [JsonPropertyName("idEmploye")]
    public int IdEmploye { get; set; }

    [JsonPropertyName("localisation")]
    public LocalisationDto? Localisation { get; set; }

    [JsonPropertyName("immobilisationImmatriculation")]
    public ImmobilisationImmatriculationDto? ImmobilisationImmatriculation { get; set; }

    [JsonPropertyName("employe")]
    public EmployeDto? Employe { get; set; }
}