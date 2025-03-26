using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
    }
}