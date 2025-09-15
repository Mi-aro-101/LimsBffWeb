using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LimsBffWeb.Models
{
    public class SemaineDto
    {
        [JsonPropertyName("id_semaine")]
        public int id_semaine { get; set; }

        [JsonPropertyName("debutSemaine")]
        public DateTime? debutSemaine { get; set; }
        
        [JsonPropertyName("finSemaine")]
        public DateTime? finSemaine { get; set; }

        [JsonPropertyName("responsable")]
        public string responsable { get; set; } = string.Empty;
    }
}