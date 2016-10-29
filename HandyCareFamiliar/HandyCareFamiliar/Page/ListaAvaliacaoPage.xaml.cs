using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace HandyCareFamiliar.Page
{
    public partial class ListaAvaliacaoPage : ContentPage
    {
        public ListaAvaliacaoPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lstCuidadores?.ClearValue(ListView.SelectedItemProperty);
        }
    }
}
