using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class BuscarPageModel:FreshBasePageModel
    {
        private Cuidador _selectedCuidador;
        private Geoname _selectedEstado;
        public ObservableCollection<Cuidador> Cuidadores { get; set; }
        public ObservableCollection<Geoname> ListaEstados { get; set; }
        public ObservableCollection<Geoname> ListaCidades { get; set; }
        public PageModelHelper PageModelHelper { get; set; }
        public Familiar Familiar { get; set; }
        public Geoname Cidade { get; set; }
        public override async void Init(object initData)
        {
            base.Init(initData);
            PageModelHelper=new PageModelHelper();
            PageModelHelper.Visualizar = false;
            PageModelHelper.ActivityRunning = true;
            PageModelHelper.CuidadorExibicao = false;
            Cidade = new Geoname();
            Familiar =new Familiar();


                var x = new HttpClient();
                var b = await x.GetStringAsync("http://www.geonames.org/childrenJSON?geonameId=3469034");
            var o = JsonConvert.DeserializeObject<RootObject>(b);
            ListaEstados=new ObservableCollection<Geoname>(o.geonames);
            PageModelHelper.Visualizar = true;
            PageModelHelper.ActivityRunning = false;
            Familiar = initData as Familiar;
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
                    await CoreMethods.PushPageModel<DetalheBuscaPageModel>(x);
                    cuidador = null;
                });
            }
        }
        public Geoname SelectedEstado
        {
            get { return _selectedEstado; }
            set
            {
                _selectedEstado = value;
                if (value != null)
                {
                    EstadoSelected.Execute(value);
                }
            }
        }

        public Command<Geoname> EstadoSelected
        {
            get
            {
                return new Command<Geoname>(async estado =>
                {
                    var x = new HttpClient();
                    var b = await x.GetStringAsync("http://www.geonames.org/childrenJSON?geonameId="+estado.geonameId);
                    var o = JsonConvert.DeserializeObject<RootObject>(b);
                    ListaCidades = new ObservableCollection<Geoname>(o.geonames);
                });
            }
        }


        public Command BuscarCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await GetCuidadores();
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
                        .Where(e=>e.CuiCidade==Cidade.name)
                        .Where(e=>e.CuiEstado==SelectedEstado.name)
                        .OrderBy(e=>e.CuiNome)
                        .AsQueryable();
                    Cuidadores = new ObservableCollection<Cuidador>(x);
                    if (Cuidadores.Count == 0)
                    {
                        PageModelHelper.CuidadorExibicao = true;

                    }
                    else
                    {
                        PageModelHelper.CuidadorExibicao = false;

                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
