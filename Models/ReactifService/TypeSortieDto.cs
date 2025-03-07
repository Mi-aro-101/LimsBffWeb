using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.ReactifService
{
    public class TypeSortieDto
    {
        [JsonPropertyName("idTypeSortie")]
        public int IdTypeSortie { get; set; }

        [JsonPropertyName("designation")]
        public string? Designation { get; set; }
    }
}
