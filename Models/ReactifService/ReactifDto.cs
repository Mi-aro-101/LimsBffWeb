using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.ReactifService
{
    public class ReactifDto
    {
        [JsonPropertyName("idReactif")]
        public int IdReactif { get; set; }

        [JsonPropertyName("designation")]
        public required string Designation { get; set; }

        [JsonPropertyName("idTypeSortie")]
        public int IdTypeSortie { get; set; }

        [JsonPropertyName("typeSortie")]
        public TypeSortieDto? TypeSortie { get; set; }

        [JsonPropertyName("idUnite")]
        public int IdUnite { get; set; }

        [JsonPropertyName("unite")]
        public UniteDto? Unite { get; set; }
    }
}
