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
    public class CameraPageModel:FreshBasePageModel
    {
        public Camera Camera { get; set; }
        public Familiar Familiar { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            Camera = new Camera();
            Camera = initData as Camera;
        }
        public Command ConfigurarCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<ConfigurarCameraPageModel>(Camera);
                });
            }
        }
    }
}
