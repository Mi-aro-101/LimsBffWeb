using System.Text.Json.Serialization;
using LimsBffWeb.Models.Immobilisation;

namespace LimsBffWeb.Models.Outillage
{
    public class ReportOutillageDto
    {
        [JsonPropertyName("idReportOutillage")]
        public int IdReportOutillage { get; set; }

        [JsonPropertyName("dateReport")]
        public DateTime DateReport { get; set; }

        [JsonPropertyName("quantite")]
        public int Quantite { get; set; }

        [JsonPropertyName("idOutillage")]
        public int IdOutillage { get; set; }

        [JsonPropertyName("outillage")]
        public OutillageDto? Outillage { get; set; }
    }
}
