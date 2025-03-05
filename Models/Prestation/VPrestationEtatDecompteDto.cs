using System.Text.Json.Serialization;

namespace LimsBffWeb.Models;

public class VPrestationEtatDecompteDto
{
    [JsonPropertyName("id_prestation")]
    public int IdPrestation { get; set; }
    [JsonPropertyName("datePrestation")]
    public required DateTime DatePrestation { get; set; }
    [JsonPropertyName("reference")]
    public required string Reference { get; set; }
    [JsonPropertyName("montant")]
    public required decimal Montant { get; set; }
    [JsonPropertyName("remise")]
    public required double Remise { get; set; }
    [JsonPropertyName("montantTotal")]
    public required decimal MontantTotal { get; set; }
    [JsonPropertyName("idEtatPrestation")]
    public int IdEtatPrestation { get; set; }
    [JsonPropertyName("designationEtat")]
    public required string DesignationEtat { get; set; }
}