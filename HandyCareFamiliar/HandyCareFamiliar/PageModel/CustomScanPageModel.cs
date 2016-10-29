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
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace HandyCareFamiliar.PageModel
{

    [ImplementPropertyChanged]
    public class CustomScanPageModel:FreshBasePageModel
    {
        public Result Teste { get; set; }
        public Familiar Familiar { get; set; }
        public Paciente Paciente { get; set; }
        private Tuple<Paciente, bool, Familiar> oi;
        public PageModelHelper PageModelHelper { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            Familiar = new Familiar();
            Familiar = initData as Familiar;
        }
        public Command<Result> ScanCommand
        {
            get
            {
                return new Command<Result>(scan =>
                {
                    PageModelHelper.Visualizar = false;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Run(async () =>
                        {
                            Teste = scan;
                            Paciente = new ObservableCollection<Paciente>(
                                await FamiliarRestService.DefaultManager.RefreshPacienteAsync())
                                .FirstOrDefault(e => e.Id == Teste.Text);
                        });
                        oi = new Tuple<Paciente, bool, Familiar>(Paciente, false, Familiar);
                        await CoreMethods.PushPageModel<PacientePageModel>(oi);
                    });
                });
            }
        }

    }
}
