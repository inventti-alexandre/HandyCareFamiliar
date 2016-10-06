using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HandyCareFamiliar.Droid.Services
{
    public class HorarioAfazerBinder:Binder
    {
        public AfazerService Service => service;

        protected AfazerService service;
        public  bool IsBound { get; set; }
        public HorarioAfazerBinder(AfazerService service)
        {
            this.service = service;
        }
    }
}