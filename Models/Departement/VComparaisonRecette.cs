namespace LimsBffWeb.Models;

public class VComparaisonRecetteDto
{
    public int Annee { get; set; }
    public int IdDepartement { get; set; }
    public string Designation { get; set; } = string.Empty;
    public decimal ChiffreAffaire { get; set; }
    public decimal Prevision { get; set; }
}