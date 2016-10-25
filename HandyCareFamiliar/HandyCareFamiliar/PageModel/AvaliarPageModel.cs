using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class AvaliarPageModel:FreshBasePageModel
    {
        public Cuidador Cuidador { get; set; }
        public Familiar Familiar { get; set; }
        public Avaliacao Avaliacao { get; set; }
        public PageModelHelper PageModelHelper { get; set; }

        public Command AvaliarCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Avaliacao.Id=new Guid().ToString();
                    await FamiliarRestService.DefaultManager.SaveAvaliacaoAsync(Avaliacao, false);
                    await CoreMethods.PopPageModel();
                });
            }

        }
        public override void Init(object initData)
        {
            base.Init(initData);
            var x = initData as Tuple<Cuidador, Familiar>;
            Cuidador=new Cuidador();
            Familiar=new Familiar();
            if (x == null) return;
            Familiar = x.Item2;
            Cuidador = x.Item1;
            Avaliacao = new Avaliacao
            {
                AvaCuidador = Cuidador.Id,
                AvaFamiliar = Familiar.Id
            };
            PageModelHelper = new PageModelHelper {BoasVindas = "Avalie o Cuidaddor " + Cuidador.CuiNomeCompleto};
        }
    }
}
