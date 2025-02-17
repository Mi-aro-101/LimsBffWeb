using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class ClientDto
{
    [JsonPropertyName("idClient")]
    public int IdClient { get; set; }
    [JsonPropertyName("nom")]
    public string Nom { get; set; }
    [JsonPropertyName("adresse")]
    public string Adresse { get; set; }
    [JsonPropertyName("cin")]
    public string? Cin { get; set; }
    [JsonPropertyName("passeport")]
    public string? Passeport { get; set; }
    [JsonPropertyName("contact")]
    public string Contact { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("fax")]
    public string? Fax { get; set; }
    [JsonPropertyName("isInterne")]
    public int IsInterne { get; set; }
    [JsonPropertyName("refContrat")]
    public string? RefContrat { get; set; }
    [JsonPropertyName("nifStat")]
    public string? NifStat { get; set; }
}