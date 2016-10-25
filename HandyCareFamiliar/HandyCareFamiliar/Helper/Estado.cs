using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace HandyCareFamiliar.Helper
{
    [ImplementPropertyChanged]
    public class Estado
    {
        public string Sigla { get; set; }
    }
}
