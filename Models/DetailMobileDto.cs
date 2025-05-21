using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class DetailMobileDto
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
        [JsonPropertyName("operateur")]
        public string? Operateur { get; set; }
        [JsonPropertyName("nombrePaiements")]
        public int NombrePaiements { get; set; }
        [JsonPropertyName("montantTotal")]
        public double MontantTotal { get; set; }
    }
}