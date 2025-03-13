using System.Text.Json.Serialization;
namespace LimsBffWeb.Models
{
    public class LocalisationDto
    {
        [JsonPropertyName("idLocalisation")]
        public int IdLocalisation { get; set; }

        [JsonPropertyName("designation")]
        public required string Designation { get; set; }
    }
}
