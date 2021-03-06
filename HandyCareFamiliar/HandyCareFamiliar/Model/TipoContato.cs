using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{
    [Table("TipoContato")]
    [ImplementPropertyChanged]
    public class TipoContato
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoContato()
        {
            ContatoEmergencia = new HashSet<ContatoEmergencia>();
        }
        public string Id { get; set; }
        public string TipDescricao { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContatoEmergencia> ContatoEmergencia { get; set; }
    }
}