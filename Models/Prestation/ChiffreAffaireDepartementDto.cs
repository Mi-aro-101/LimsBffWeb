using System.Text.Json.Serialization;
using LimsBffWeb.Models;

namespace LimsBffWeb.Models;

public class ChiffreAffaireDepartementDto
{
    [JsonPropertyName("idDepartement")]
    public int? IdDepartement { get; set; }
    [JsonPropertyName("designation")]
    public string? Designation { get; set; }
    [JsonPropertyName("chiffreAffaires")]
    public ICollection<ChiffreAffaireDto> ChiffreAffaires { get; set; } = new List<ChiffreAffaireDto>();
}