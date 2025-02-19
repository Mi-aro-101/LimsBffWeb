using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class AvanceeTravailDto
{
    [JsonPropertyName("idAvanceeTravail")]
    public int IdAvanceeTravail { get; set; }
    [JsonPropertyName("niveau")]
    public int Niveau { get; set; }    
    [JsonPropertyName("designation")]
    public string Designation { get; set; }
}