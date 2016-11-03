using Xamarin.Forms;

namespace HandyCareFamiliar.Page
{
    public partial class AcionarContatoEmergenciaPage : ContentPage
    {
        public AcionarContatoEmergenciaPage()
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