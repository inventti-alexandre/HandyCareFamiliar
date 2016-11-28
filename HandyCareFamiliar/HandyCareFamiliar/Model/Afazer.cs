using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{
    [Table("Afazer")]
    [ImplementPropertyChanged]
    public class Afazer
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Afazer()
        //{
        //    ConclusaoAfazer = new HashSet<ConclusaoAfazer>();
        //    MaterialUtilizado = new HashSet<MaterialUtilizado>();
        //    MedicamentoAdministrado = new HashSet<MedicamentoAdministrado>();
        //}
        public string Id { get; set; }

        public DateTime AfaHorarioPrevisto { get; set; }

        public string AfaObservacao { get; set; }
        public string AfaPaciente { get; set; }
        [JsonIgnore]
        public string AfaDescricaoMotivoNaoValidacaoAfazer { get; set; }
        public string AfaTitulo { get; set; }
        public bool AfaRecorrente { get; set; }
        public DateTime AfaHorarioPrevistoTermino { get; set; }
        public string AfaCor { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ConclusaoAfazer> ConclusaoAfazer { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MaterialUtilizado> MaterialUtilizado { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MedicamentoAdministrado> MedicamentoAdministrado { get; set; }
    }
}