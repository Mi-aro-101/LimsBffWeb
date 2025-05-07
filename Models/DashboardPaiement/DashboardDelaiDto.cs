using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class DashboardDelaiDto
    {
        [JsonPropertyName("nombreEnEspece")]
        public int nombreEnEspece { get; set; }
        [JsonPropertyName("nombreEnMobile")]
        public int nombreEnMobile { get; set; }
        [JsonPropertyName("montantEnEspece")]
        public double montantEnEspece { get; set; }
        [JsonPropertyName("montantEnMobile")]
        public double montantEnMobile { get; set; }
        [JsonPropertyName("dateDujour")]
        public DateTime? dateDujour { get; set; }
    }
}