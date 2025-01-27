using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;
public class RoleDto
{
    [JsonPropertyName("idRole")]
    public int IdRole {get; set; }

    [JsonPropertyName("designation")]
    public string Designation { get; set; }
}