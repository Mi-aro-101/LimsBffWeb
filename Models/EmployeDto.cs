using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class EmployeDto
{
    [JsonPropertyName("idEmploye")]
    public int IdEmploye { get; set; }
    [JsonPropertyName("matricule")]
    public string? Matricule { get; set; }
    [JsonPropertyName("nom")]
    public string? Nom { get; set; }
    [JsonPropertyName("prenom")]
    public string? Prenom { get; set; }
    [JsonPropertyName("genre")]
    public int Genre { get; set; }
    [JsonPropertyName("cin")]
    public string? Cin { get; set; }
    [JsonPropertyName("contact")]
    public string? Contact { get; set; }
    [JsonPropertyName("adresse")]
    public string? Adresse { get; set; }
    [JsonPropertyName("manager")]
    public string? Manager { get; set; }
    [JsonPropertyName("idDepartement")]
    public int? IdDepartement { get; set; }
    [JsonPropertyName("departement")]
    public DepartementDto? Departement { get; set; }
    [JsonPropertyName("idPoste")] 
    public int IdPoste { get; set; }
    [JsonPropertyName("poste")]
    [JsonIgnore]
    public DateOnly? _dateNouveauPoste;
    [JsonIgnore]
    public DateOnly? _dateFinPoste;
    [JsonPropertyName("dateNouveauPoste")]
    public DateOnly? DateNouveauPoste { 
        get => _dateNouveauPoste;
        set {
            if(value > DateOnly.FromDateTime(DateTime.Now)){
                throw new ArgumentException("Date de nouveau poste ne peut pas être dans le futur.");
            }
            if(_dateFinPoste != null){
                if(value < _dateFinPoste.Value){
                    throw new ArgumentException("Date de fin de poste ne peut pas être antérieure à la date de début de poste.");
                }
            }
            _dateNouveauPoste = value;
        }
    }
    [JsonPropertyName("dateFinAncienPoste")]
    public DateOnly? DateFinPoste { 
        get => _dateFinPoste;
        set => _dateFinPoste = value;
    }
    [JsonPropertyName("historiqueEmployes")]
    public ICollection<HistoriqueEmployeDto> HistoriqueEmployes = new List<HistoriqueEmployeDto>();
}