using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{
    [Table("Familiar")]
    [ImplementPropertyChanged]
    public class Familiar
    {

        public string Id { get; set; }
        public string FamParentesco { get; set; }

        public string FamContatoEmergencia { get; set; }

        public string FamNome { get; set; }
        public string FamSobrenome { get; set; }

        [JsonIgnore]
        public string FamNomeCompleto => FamNome + " " + FamSobrenome;

        [JsonProperty(PropertyName = "FamGoogleId")]
        public string FamGoogleId { get; set; }
        public string FamCidade { get; set; }
        public string FamEstado { get; set; }

        [JsonProperty(PropertyName = "FamFacebookId")]
        public string FamFacebookId { get; set; }

        [JsonProperty(PropertyName = "FamMicrosoftId")]
        public string FamMicrosoftId { get; set; }

        [JsonProperty(PropertyName = "FamMicrosoftAdId")]
        public string FamMicrosoftAdId { get; set; }

        [JsonProperty(PropertyName = "FamTwitterId")]
        public string FamTwitterId { get; set; }
        public string FamInstallationId { get; set; }
        public string FamRegistrationId { get; set; }
        public byte[] FamFoto { get; set; }

        public virtual Parentesco Parentesco { get; set; }

    }
}