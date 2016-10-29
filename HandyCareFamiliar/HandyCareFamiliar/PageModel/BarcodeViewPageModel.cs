using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Model;
using PropertyChanged;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class BarcodeViewPageModel:FreshBasePageModel
    {
        public Paciente Paciente { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            Paciente = new Paciente();
            Paciente = initData as Paciente;
        }

    }
}
