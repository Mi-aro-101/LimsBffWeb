using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class DashboardDelaiMobileDto
    {
        [JsonPropertyName("semaines")]
        public List<SemaineDto>? Semaines { get; set; }     
        [JsonPropertyName("details")]   
        public List<DelaiMobileDto>? Details { get; set; }
    }
}