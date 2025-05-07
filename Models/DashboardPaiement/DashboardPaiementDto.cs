using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class DashboardPaiementDto
    {
        [JsonPropertyName("paiements")]
        public List<RecuDto>? Paiements { get; set; }
        [JsonPropertyName("semaines")]
        public List<SemaineDto>? Semaines { get; set; }
    }
}