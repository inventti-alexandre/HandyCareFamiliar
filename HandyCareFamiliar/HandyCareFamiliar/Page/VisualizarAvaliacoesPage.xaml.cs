using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace HandyCareFamiliar.Page
{
    public partial class VisualizarAvaliacoesPage : ContentPage
    {
        public VisualizarAvaliacoesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lstAvaliacoes?.ClearValue(ListView.SelectedItemProperty);
        }
    }
}
