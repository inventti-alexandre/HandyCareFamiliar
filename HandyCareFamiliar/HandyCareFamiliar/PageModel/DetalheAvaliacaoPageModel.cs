using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Model;
using PropertyChanged;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class DetalheAvaliacaoPageModel:FreshBasePageModel
    {
        public Avaliacao Avaliacao { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            Avaliacao=new Avaliacao();
            Avaliacao = initData as Avaliacao;
        }
    }
}
