using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class EchantillonDto
{
    [JsonPropertyName("idEchantillon")]
    public int? IdEchantillon { get; set; }
    [JsonPropertyName("note")]
    public string? Note { get; set; } 
    [JsonPropertyName("provenance")]
    public string? Provenance { get; set; }
    [JsonPropertyName("datePrelevement")]
    public DateOnly? DatePrelevement { get; set; }
    [JsonPropertyName("idTypeEchantillon")]
    public int IdTypeEchantillon { get; set; }
    [JsonPropertyName("typeTravaux")]
    public ICollection<TravailDto> TypeTravaux { get; set; } = new List<TravailDto>();

    [JsonPropertyName("typeEchantillon")]
    public TypeEchantillonDto? TypeEchantillon { get; set; }
    [JsonPropertyName("idPreleveur")]
    public int? IdPreleveur { get; set; }
    [JsonPropertyName("preleveur")]
    public PreleveurDto? Preleveur { get; set; }

    // For details
    [JsonPropertyName("detailsEchantillons")]
    public ICollection<VDetailsEchantillonDto> DetailsEchantillons { get; set; } = new List<VDetailsEchantillonDto>();
}