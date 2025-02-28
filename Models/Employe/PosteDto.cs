using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class PosteDto
{
    [JsonPropertyName("idPoste")]
    public int IdPoste { get; set; }
    
    [JsonPropertyName("designation")]
    public string? Designation { get; set; }
}