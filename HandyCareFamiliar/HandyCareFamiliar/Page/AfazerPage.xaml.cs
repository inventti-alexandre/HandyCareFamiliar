using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace HandyCareFamiliar.Page
{
    public partial class AfazerPage : ContentPage
    {
        public bool deleteVisible;

        public AfazerPage()
        {
            try

            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
               Debug.WriteLine(ex.ToString());
            }
        }

        /* protected override bool OnBackButtonPressed()
         {
             lstAfazer.SelectedItem = null;
             return base.OnBackButtonPressed();
         }*/
    }
}