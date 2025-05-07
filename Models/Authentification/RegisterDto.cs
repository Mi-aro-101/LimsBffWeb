using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;
public class RegisterDto {

    [JsonPropertyName("identifiant")]
     public required string Identifiant { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }

    [JsonPropertyName("roles")]
    public required List<int> Roles { get; set; } // Optional: Only if assigning roles during registration

    [JsonPropertyName("idDepartement")]
    public required int IdDepartement { get; set; }
}