using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ListaAvaliacaoPageModel:FreshBasePageModel
    {
        private Cuidador _selectedCuidador;
        public ObservableCollection<Cuidador> Cuidadores { get; set; }
        public Familiar Familiar { get; set; }
        public PacienteFamiliar PacienteFamiliar { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public PageModelHelper PageModelHelper { get; set; }
        public override async void Init(object initData)
        {
            base.Init(initData);
            Familiar=new Familiar();
            var x = initData as Tuple<PacienteFamiliar>;
            await GetCuidadores();
        }

        public Cuidador SelectedCuidador
        {
            get { return _selectedCuidador; }
            set
            {
                _selectedCuidador = value;
                if (value != null)
                {
                    AfazerSelected.Execute(value);
                    SelectedCuidador = null;
                }
            }
        }

        public Command<Cuidador> AfazerSelected
        {
            get
            {
                return new Command<Cuidador>(async cuidador =>
                {
                    var x = new Tuple<Cuidador, Familiar>(cuidador, Familiar);
                    await CoreMethods.PushPageModel<AvaliarPageModel>(x);
                    cuidador = null;
                });
            }
        }

        private async Task GetCuidadores()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var x =
                        new ObservableCollection<Cuidador>(await FamiliarRestService.DefaultManager
                        .RefreshCuidadorAsync(true))
                        .Where(e => e.CuiCidade == PageModelHelper.Cidade).AsQueryable();
                    Cuidadores = new ObservableCollection<Cuidador>(x);
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
