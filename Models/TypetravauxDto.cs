using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class TypeTravauxDto
{
    [JsonPropertyName("idTypeTravaux")]
    public int IdTypeTravaux { get; set; }
    [JsonPropertyName("code")]
    public string Code { get; set; }
    [JsonPropertyName("designation")]
    public string Designation { get; set; }
    [JsonPropertyName("hasResultat")]
    public int HasResultat { get; set; }
    [JsonPropertyName("idDepartement")]
    public int IdDepartement { get; set; }
    [JsonPropertyName("departement")]
    public DepartementDto? Departement { get; set; }
    [JsonPropertyName("tarif")]
    public decimal? Tarif { get; set; }
    [JsonPropertyName("dateCreation")]
    public DateTime? DateCreation { get; set; }

}