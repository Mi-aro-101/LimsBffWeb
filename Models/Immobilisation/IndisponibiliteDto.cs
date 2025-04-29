using System.Text.Json.Serialization;

namespace LimsBffWeb.Models.Immobilisation;

     public class IndisponibiliteDto
    {
        [JsonPropertyName("idIndisponibilite")]
        public int IdIndisponibilite { get; set; }

        [JsonPropertyName("dateDebut")]
        public DateTime DateDebut { get; set; }

        [JsonPropertyName("dateFin")]
        public DateTime DateFin { get; set; }

        [JsonPropertyName("idImmobilisationPropre")]
        public int IdImmobilisationPropre { get; set; }

        [JsonPropertyName("idObjetIndisponibilite")]
        public int IdObjetIndisponibilite { get; set; }

        [JsonPropertyName("immobilisationImmatriculation")]
        public ImmobilisationImmatriculationDto? ImmobilisationImmatriculation { get; set; }

        [JsonPropertyName("objetIndisponibilite")]
        public ObjetIndisponibiliteDto? ObjetIndisponibilite { get; set; }
    }