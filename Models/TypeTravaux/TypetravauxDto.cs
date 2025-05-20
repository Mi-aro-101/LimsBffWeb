using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class TypeTravauxDto
{
    [JsonPropertyName("idTypeTravaux")]
    public int IdTypeTravaux { get; set; }
    [JsonPropertyName("code")]
    public required string Code { get; set; }
    [JsonPropertyName("designation")]
    public required string Designation { get; set; }
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
    [JsonPropertyName("dateChangement")]
    public DateTime? DateChangement { get; set; }
    [JsonPropertyName("historiqueTarifs")]
    public ICollection<HistoriqueTarifDto> HistoriqueTarifs { get; set; } = new List<HistoriqueTarifDto>();
    [JsonPropertyName("haveFormule")]
    public int HaveFormule { get; set; }
    [JsonPropertyName("formuleString")]
    public string? FormuleString { get; set; }
    [JsonPropertyName("formuleBytes")]
    public byte[]? FormuleBytes { get; set; }
    [JsonPropertyName("idTypeEchantillons")]
    public ICollection<int>? IdEchantillons { get; set; } = new List<int>();

    [JsonPropertyName("typeTravauxTypeEchantillons")]
    public ICollection<TypeTravauxTypeEchantillonDto>? TypeTravauxTypeEchantillonDto {get; set; } = new List<TypeTravauxTypeEchantillonDto>();

}