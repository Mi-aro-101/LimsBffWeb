using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.ReactifService
{
    public class DepartementReactifDto
    {
        [JsonPropertyName("idDepartement")]
        public int IdDepartement { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("designation")]
        public string? Designation { get; set; }
    }
}
