using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class PrestationDto
{
    [JsonPropertyName("id_prestation")]
    public int IdPrestation { get; set; }
    [JsonPropertyName("reference_fiche_travail")]
    public required string ReferenceFicheTravail { get; set; }
    [JsonPropertyName("total_montant")]
    public required double TotalMontant { get; set; }
    [JsonPropertyName("remise")]
    public required double Remise { get; set; }
    [JsonPropertyName("date_prestation")]
    public required DateTime DatePrestation { get; set; }
    [JsonPropertyName("date_cloture")]
    public DateTime? DateCloture { get; set; }
    [JsonPropertyName("id_etat_prestation")]
    public int IdEtatPrestation { get; set; }
    [JsonPropertyName("IdEtatPrestation")]
    public EtatPrestationDto? EtatPrestation { get; set; }
    [JsonPropertyName("id_client")]
    public int IdClient { get; set; }
    [JsonPropertyName("IdClient")]
    public ClientDto? Client { get; set; }
    [JsonPropertyName("statut_paiement")]
    public int StatutPaiement { get; set; }
}