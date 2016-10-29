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
    public class ListaCameraPageModel:FreshBasePageModel
    {
        private Camera _selectedCamera;
        public ObservableCollection<Camera> Cameras { get; set; }
        public Familiar Familiar { get; set; }
        public PageModelHelper PageModelHelper { get; set; }

        public Command AddCamera
        {
            get
            {
                return new Command(async () =>
                {
                    //var x = new Tuple<Camera, Paciente, PacienteFamiliar>(null, oPaciente, PacienteFamiliar);
                    //await CoreMethods.PushPageModel<CameraPageModel>(x);
                });
            }
        }

        public Camera SelectedCamera
        {
            get { return _selectedCamera; }
            set
            {
                _selectedCamera = value;
                if (value != null)
                {
                    CameraSelected.Execute(value);
                    SelectedCamera = null;
                }
            }
        }

        public Command<Camera> CameraSelected
        {
            get
            {
                return new Command<Camera>(async camera =>
                {
                    await CoreMethods.PushPageModel<CameraPageModel>(camera);
                    camera = null;
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            Familiar = new Familiar();
            Familiar = initData as Familiar;
            PageModelHelper = new PageModelHelper
            {
                ActivityRunning = true,
                Visualizar = false
            };
            await GetCameras();
        }

        private async Task GetCameras()
        {
            await Task.Run(async () =>
            {
                var camresult = new ObservableCollection<Camera>(
                    await FamiliarRestService.DefaultManager.RefreshCameraAsync(true))
                    .Where(e => e.CamFamiliar == Familiar.Id)
                    .AsEnumerable();
                Cameras = new ObservableCollection<Camera>(camresult);
                PageModelHelper.ActivityRunning = false;
                PageModelHelper.Visualizar = true;
            });
        }
    }
}
