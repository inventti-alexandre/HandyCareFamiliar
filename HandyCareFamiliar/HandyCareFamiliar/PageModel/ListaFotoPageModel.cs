using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class ListaFotoPageModel : FreshBasePageModel
    {
        private Foto _selectedFoto;
        public Familiar Familiar { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ObservableCollection<FotoFamiliar> FotoFamiliar { get; set; }
        public ObservableCollection<Foto> Fotos { get; set; }

        public Foto SelectedFoto
        {
            get { return _selectedFoto; }
            set
            {
                _selectedFoto = value;
                if (value == null) return;
                FotoSelected.Execute(value);
                SelectedFoto = null;
            }
        }

        public Command<Foto> FotoSelected
        {
            get
            {
                return new Command<Foto>(async foto =>
                {
                    RaisePropertyChanged("IsVisible");
                    await CoreMethods.PushPageModel<FotoPageModel>(foto);
                    foto = null;
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            oHorario = new PageModelHelper {ActivityRunning = true};
            Familiar = new Familiar();
            Familiar = initData as Familiar;
            GetFotos();
        }

        private async void GetFotos()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var x =
                        new ObservableCollection<FotoFamiliar>(
                                await FamiliarRestService.DefaultManager.RefreshFotoFamiliarAsync()).
                            Where(e => e.FamId == Familiar.Id)
                            .AsEnumerable();
                    FotoFamiliar = new ObservableCollection<FotoFamiliar>(x);
                    var y = new ObservableCollection<Foto>(await FamiliarRestService.DefaultManager.RefreshFotoAsync()).
                        Where(e => x.Select(b => b.FotId).Contains(e.Id))
                        .AsEnumerable();
                    Fotos = new ObservableCollection<Foto>(y.OrderBy(e => e.CreatedAt));
                    oHorario.ActivityRunning = false;
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}