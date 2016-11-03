using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{
    [Table("MotivoNaoValidacaoConclusaoAfazer")]
    [ImplementPropertyChanged]
    public class MotivoNaoValidacaoConclusaoAfazer
    {
        public string Id { get; set; }
        public string MoDescricao { get; set; }

        public string MoValidacao { get; set; }

        public virtual ValidacaoAfazer ValidacaoAfazer { get; set; }
    }
}