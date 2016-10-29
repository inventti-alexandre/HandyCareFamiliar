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
            PacienteFamiliar=new PacienteFamiliar();
            var x = initData as Tuple<Familiar,PacienteFamiliar>;
            if (x != null)
            {
                Familiar = x.Item1;
                PacienteFamiliar = x.Item2;
            }
        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            PageModelHelper = new PageModelHelper { ActivityRunning = true };
            await GetCuidadores();
            PageModelHelper.ActivityRunning = false;
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
                    var y = new ObservableCollection<CuidadorPaciente>(await FamiliarRestService.DefaultManager
                        .RefreshCuidadorPacienteAsync(true))
                        .Where(e => e.PacId == PacienteFamiliar.PacId).AsQueryable();
                    var x =
                        new ObservableCollection<Cuidador>(await FamiliarRestService.DefaultManager
                        .RefreshCuidadorAsync(true))
                        .Where(e => y.Select(i=>i.CuiId).Contains(e.Id)).AsQueryable();
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
