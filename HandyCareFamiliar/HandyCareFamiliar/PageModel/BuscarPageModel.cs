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
    public class BuscarPageModel:FreshBasePageModel
    {
        public ObservableCollection<Cuidador> Cuidadores { get; set; }
        public PageModelHelper PageModelHelper { get; set; }
        public override async void Init(object initData)
        {
            base.Init(initData);
            PageModelHelper=new PageModelHelper();
            var x = initData as Familiar;
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
                        .Where(e=>e.CuiCidade==PageModelHelper.Cidade).AsQueryable();
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
