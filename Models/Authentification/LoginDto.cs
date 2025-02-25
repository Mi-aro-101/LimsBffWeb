using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;
public class LoginDto
{
    [JsonPropertyName("identifiant")]
    public string Identifiant { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}