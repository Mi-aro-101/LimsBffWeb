using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.ReactifService
{
    public class ReportReactifDto
    {
        [JsonPropertyName("idReportReactif")]
        public int IdReportReactif { get; set; }

        [JsonPropertyName("dateReport")]
        public DateTime DateReport { get; set; }

        [JsonPropertyName("quantite")]
        public double Quantite { get; set; }

        [JsonPropertyName("idReactif")]
        public int IdReactif { get; set; }

        [JsonPropertyName("reactif")]
        public ReactifDto? Reactif { get; set; }
    }
}
