using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace HandyCareFamiliar.Page
{
    public partial class ListaCameraPage : ContentPage
    {
        public ListaCameraPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
                lstCamera.ClearValue(ListView.SelectedItemProperty);
        }

    }
}
