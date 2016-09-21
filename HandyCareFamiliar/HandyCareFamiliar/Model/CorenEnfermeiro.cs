using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{
    [Table("CorenEnfermeiro")]
    [ImplementPropertyChanged]
    public class CorenEnfermeiro
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CorenEnfermeiro()
        {
            ValidacaoCuidador = new HashSet<ValidacaoCuidador>();
        }

        public string CorenIdentificacao { get; set; }

        public bool CorenValidado { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ValidacaoCuidador> ValidacaoCuidador { get; set; }
    }
}