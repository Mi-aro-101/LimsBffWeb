using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;
public class RegisterDto {

    [JsonPropertyName("identifiant")]
     public string Identifiant { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; } // Optional: Only if assigning roles during registration
}