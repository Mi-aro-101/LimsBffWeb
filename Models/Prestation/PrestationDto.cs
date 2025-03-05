using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class PrestationDto
{
    [JsonPropertyName("idPrestation")]
    public int IdPrestation { get; set; }
    [JsonPropertyName("referenceFicheTravail")]
    public required string ReferenceFicheTravail { get; set; }
    [JsonPropertyName("datePrestation")]
    public required DateTime DatePrestation { get; set; }
    [JsonPropertyName("dateCloture")]
    public DateTime? DateCloture { get; set; }
    [JsonPropertyName("idEtatPrestation")]
    public int IdEtatPrestation { get; set; }
    [JsonPropertyName("etatPrestation")]
    public EtatPrestationDto? EtatPrestation { get; set; }
    [JsonPropertyName("idClient")]
    public int IdClient { get; set; }
    [JsonPropertyName("client")]
    public ClientDto? Client { get; set; }
    [JsonPropertyName("statutPaiement")]
    public int StatutPaiement { get; set; }

    public ICollection<VPrestationDetailsDto> PrestationDetails { get; set; } = new List<VPrestationDetailsDto>();
}