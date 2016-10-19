using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{
    [Table("ContatoEmergencia")]
    [ImplementPropertyChanged]
    public class ContatoEmergencia
    {
        [Column(Order = 0)]
        public string Id { get; set; }

        public string ConTelefone { get; set; }

        public string ConCelular { get; set; }

        public string ConEmail { get; set; }

        public string ConDescricao { get; set; }

        public string ConTipo { get; set; }

        public string ConPessoa { get; set; }
    }
}