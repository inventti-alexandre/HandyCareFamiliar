using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
    public class DetalheBuscaPageModel:FreshBasePageModel
    {
        public PageModelHelper PageModelHelper { get; set; }
        public Familiar Familiar { get; set; }
        public Cuidador Cuidador { get; set; }
        public Avaliacao Avaliacao { get; set; }
        public ImageSource CuidadorFoto { get; set; }

        public ImageSource Documento { get; set; }
        public ObservableCollection<Avaliacao> Avaliacoes { get; set; }

        public override async void Init(object initData)
        {
            base.Init(initData);
            PageModelHelper = new PageModelHelper
            {
                ActivityRunning = true,
                Visualizar = false
            };
            Familiar = new Familiar();
            Cuidador = new Cuidador();
            var x = initData as Tuple<Cuidador,Familiar>;
            if (x == null) return;
            Familiar = x.Item2;
            Cuidador = x.Item1;
            if (Cuidador.CuiFoto != null)
            {
                CuidadorFoto = ImageSource.FromStream(() => new MemoryStream(Cuidador.CuiFoto));
            }
            //if (Cuidador. != null)
            //{
            //    Documento = ImageSource.FromStream(() => new MemoryStream(Cuidador.CuiFoto));
            //}

            await GetAvaliacoes();
            PageModelHelper.ActivityRunning = false;
            PageModelHelper.Visualizar = true;
        }
        public Command AvaliacaoCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<VisualizarAvaliacoesPageModel>(Avaliacoes);
                });
            }
        }

        private async Task GetAvaliacoes()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var selection =
                        new ObservableCollection<Avaliacao>(
                            await FamiliarRestService.DefaultManager.RefreshAvaliacaoAsync(true))
                            .Where(e => e.AvaCuidador ==Cuidador.Id).AsEnumerable();
                    PageModelHelper.Media = 0;
                    var avaliacaos = selection as Avaliacao[] ?? selection.ToArray();
                    foreach (var x in avaliacaos)
                    {
                        PageModelHelper.Media += x.AvaPontuacao;
                    }
                    PageModelHelper.Media /= avaliacaos.Count();
                    Avaliacoes = new ObservableCollection<Avaliacao>(avaliacaos);
                });
            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

    }
}
