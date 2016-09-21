using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCareFamiliar.Model
{
    [Table("MotivoNaoValidacaoConclusaoAfazer")]
    public class MotivoNaoValidacaoConclusaoAfazer
    {
        public string MoDescricao { get; set; }

        public string MoValidacao { get; set; }

        public virtual ValidacaoAfazer ValidacaoAfazer { get; set; }
    }
}