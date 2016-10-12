using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using HandyCareFamiliar.Data;
using Microsoft.WindowsAzure.MobileServices;

namespace HandyCareFamiliar.UWP
{
    public sealed partial class MainPage : HandyCareFamiliar.App.IAuthenticate
    {
        private MobileServiceUser user;

        public MainPage()
        {
            InitializeComponent();
            HandyCareFamiliar.App.Init(this);
            LoadApplication(new HandyCareFamiliar.App());
        }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                user = await FamiliarRestService.DefaultManager.CurrentClient.LoginAsync(provider);
                if (user != null)
                {
                    message = $"you are now signed-in as {user.UserId}.";
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            // Display the success or failure message.
            await new MessageDialog(message, "Sign-in result").ShowAsync();

            return success;
        }
    }
}