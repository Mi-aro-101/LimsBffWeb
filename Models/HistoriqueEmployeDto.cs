using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class HistoriqueEmployeDto
{
    [JsonPropertyName("idHistoriqueEmploye")]
    public int IdHistoriqueEmploye { get; set; }

    [JsonPropertyName("dateDebut")]
    public DateOnly DateDebut { get; set; }
    [JsonPropertyName("dateFin")]
    public DateOnly? DateFin { get; set; }
    [JsonPropertyName("idPoste")]
    public int IdPoste { get; set; }
    [JsonPropertyName("poste")]
    public PosteDto? Poste { get; set; }
    [JsonPropertyName("idEmploye")]
    public int? IdEmploye { get; set; }
    [JsonPropertyName("employe")]
    [JsonIgnore]
    public EmployeDto? Employe { get; set; }
}