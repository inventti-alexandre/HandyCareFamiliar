using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using PropertyChanged;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class ConfigurarCameraPageModel:FreshBasePageModel
    {
        public Camera Camera { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            Camera = new Camera();
            Camera = initData as Camera;

        }
    }
}
