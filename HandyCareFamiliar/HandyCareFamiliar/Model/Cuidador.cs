using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{
    //using Microsoft.Azure.Mobile.Server;

    [Table("Cuidador")]
    [ImplementPropertyChanged]
    public class Cuidador
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cuidador()
        {
            Avaliacao = new HashSet<Avaliacao>();
            ContatoEmergencia = new HashSet<ContatoEmergencia>();
            CuidadorPaciente = new HashSet<CuidadorPaciente>();
            Foto = new HashSet<Foto>();
            Video = new HashSet<Video>();
        }

        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "CuiTipoCuidador")]
        public string CuiTipoCuidador { get; set; }

        [JsonProperty(PropertyName = "CuiValidacaoCuidador")]
        public string CuiValidacaoCuidador { get; set; }

        [JsonProperty(PropertyName = "CuiNome")]
        public string CuiNome { get; set; }

        [JsonProperty(PropertyName = "CuiSobrenome")]
        public string CuiSobrenome { get; set; }

        [JsonProperty(PropertyName = "CuiCidade")]
        public string CuiCidade { get; set; }

        [JsonProperty(PropertyName = "CuiEstado")]
        public string CuiEstado { get; set; }
        public string CuiContatoEmergencia { get; set; }

        public byte[] CuiFoto { get; set; }

        [JsonProperty(PropertyName = "CuiGoogleId")]
        public string CuiGoogleId { get; set; }

        [JsonProperty(PropertyName = "CuiFacebookId")]
        public string CuiFacebookId { get; set; }

        [JsonProperty(PropertyName = "CuiMicrosoftId")]
        public string CuiMicrosoftId { get; set; }

        [JsonProperty(PropertyName = "CuiMicrosoftAdId")]
        public string CuiMicrosoftAdId { get; set; }

        [JsonProperty(PropertyName = "CuiTwitterId")]
        public string CuiTwitterId { get; set; }

        [JsonIgnore]
        public string CuiNomeCompleto => CuiNome + " " + CuiSobrenome;


        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avaliacao> Avaliacao { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContatoEmergencia> ContatoEmergencia { get; set; }

        public virtual TipoCuidador TipoCuidador { get; set; }

        public virtual ValidacaoCuidador ValidacaoCuidador { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuidadorPaciente> CuidadorPaciente { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Foto> Foto { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video> Video { get; set; }
    }
}