using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class TypeTravauxTypeEchantillonDto
{
    [JsonPropertyName("idTypeTravaux")]
    public int IdTypeTravaux { get; set; }

    [JsonIgnore]
    public TypeTravauxDto? TypeTravaux { get; set; } 

    [JsonPropertyName("idTypeEchantillon")]
    public int IdTypeEchantillon { get; set; }
    [JsonPropertyName("typeEchantillon")]
    public TypeEchantillonDto? TypeEchantillon { get; set; }
}