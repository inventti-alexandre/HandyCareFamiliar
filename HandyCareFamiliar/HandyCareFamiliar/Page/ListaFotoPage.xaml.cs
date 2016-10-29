using Xamarin.Forms;

namespace HandyCareFamiliar.Page
{
    public partial class ListaFotoPage : ContentPage
    {
        public ListaFotoPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lstFotos?.ClearValue(ListView.SelectedItemProperty);
        }

    }
}