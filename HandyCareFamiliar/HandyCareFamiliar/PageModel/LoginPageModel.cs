using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using Microsoft.WindowsAzure.MobileServices;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class LoginPageModel : FreshBasePageModel
    {
        private App app;
        private bool authenticated;
        public Familiar Familiar { get; set; }
        public HorarioViewModel oHorarioViewModel { get; set; }

        public Command GoogleLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                        {
                            authenticated =
                                await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                        }
                        if (!authenticated) return;
                        Application.Current.Properties["UserId"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                        Application.Current.Properties["Token"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser
                                .MobileServiceAuthenticationToken;
                        App.Authenticated = true;
                        oHorarioViewModel.Visualizar = false;
                        oHorarioViewModel.ActivityRunning = true;
                        Familiar =
                            await FamiliarRestService.DefaultManager.ProcurarFamiliarAsync(FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.Google);
                        if (Familiar != null)
                        {
                            App.Afazeres = new ObservableCollection<Afazer>();
                            app.AbrirMainMenu(Familiar);
                            await App.GetAfazeres(true);
                        }
                        else
                        {
                            var _Familiar = new Familiar { FamGoogleId = FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId };
                            app.NewFamiliar(_Familiar, app);
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }
                });
            }
        }
        public Command LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                        {
                            authenticated =
                                await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                        }
                        if (!authenticated) return;
                        Application.Current.Properties["UserId"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                        Application.Current.Properties["Token"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser
                                .MobileServiceAuthenticationToken;
                        App.Authenticated = true;
                        oHorarioViewModel.Visualizar = false;
                        oHorarioViewModel.ActivityRunning = true;
                        Familiar =
                            await FamiliarRestService.DefaultManager.ProcurarFamiliarAsync(FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.Google);
                        if (Familiar != null)
                        {
                            App.Afazeres = new ObservableCollection<Afazer>();
                            app.AbrirMainMenu(Familiar);
                            await App.GetAfazeres(true);
                        }
                        else
                        {
                            var _Familiar = new Familiar { FamMicrosoftId = FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId };
                            app.NewFamiliar(_Familiar, app);
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }
                });
            }
        }
        public Command FacebookLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated =
                                await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Facebook);
                        if (!authenticated) return;
                        Application.Current.Properties["UserId"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                        Application.Current.Properties["Token"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser
                                .MobileServiceAuthenticationToken;
                        App.Authenticated = true;
                        oHorarioViewModel.Visualizar = false;
                        oHorarioViewModel.ActivityRunning = true;
                        Familiar =
                            await FamiliarRestService.DefaultManager.ProcurarFamiliarAsync(FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.Facebook, true);
                        if (Familiar != null)
                        {
                            App.Afazeres = new ObservableCollection<Afazer>();
                            app.AbrirMainMenu(Familiar);
                            await App.GetAfazeres(true);

                        }
                        else
                        {
                            var _Familiar = new Familiar { FamFacebookId = FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId };
                            app.NewFamiliar(_Familiar, app);
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }
                });
            }
        }

        public Command MicrosoftLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated =
                                await
                                    App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.MicrosoftAccount);
                        if (!authenticated) return;
                        Application.Current.Properties["UserId"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                        Application.Current.Properties["Token"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser
                                .MobileServiceAuthenticationToken;
                        App.Authenticated = true;
                        oHorarioViewModel.Visualizar = false;
                        oHorarioViewModel.ActivityRunning = true;
                        Familiar =
                            await FamiliarRestService.DefaultManager.ProcurarFamiliarAsync(FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.MicrosoftAccount, true);
                        if (Familiar != null)
                        {
                            App.Afazeres = new ObservableCollection<Afazer>();
                            app.AbrirMainMenu(Familiar);
                            await App.GetAfazeres(true);

                        }
                        else
                        {
                            var _Familiar = new Familiar {FamMicrosoftId = FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId};
                            app.NewFamiliar(_Familiar, app);
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }
                });
            }
        }
        public Command TwitterLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated =
                                await
                                    App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Twitter);
                        if (!authenticated) return;
                        Application.Current.Properties["UserId"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                        Application.Current.Properties["Token"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser
                                .MobileServiceAuthenticationToken;
                        App.Authenticated = true;
                        oHorarioViewModel.Visualizar = false;
                        oHorarioViewModel.ActivityRunning = true;
                        Familiar =
                            await FamiliarRestService.DefaultManager.ProcurarFamiliarAsync(FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.Twitter, true);
                        if (Familiar != null)
                        {
                            App.Afazeres = new ObservableCollection<Afazer>();
                            app.AbrirMainMenu(Familiar);
                            await App.GetAfazeres(true);

                        }
                        else
                        {
                            var _Familiar = new Familiar { FamTwitterId = FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId };
                            app.NewFamiliar(_Familiar, app);
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }
                });
            }
        }

        public Command AzureAdLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated =
                                await
                                    App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                        if (!authenticated) return;
                        Application.Current.Properties["UserId"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                        Application.Current.Properties["Token"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser
                                .MobileServiceAuthenticationToken;
                        App.Authenticated = true;
                        oHorarioViewModel.Visualizar = false;
                        oHorarioViewModel.ActivityRunning = true;
                        Familiar =
                            await FamiliarRestService.DefaultManager.ProcurarFamiliarAsync(FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, true);
                        if (Familiar != null)
                        {
                            App.Afazeres = new ObservableCollection<Afazer>();
                            app.AbrirMainMenu(Familiar);
                            await App.GetAfazeres(true);

                        }
                        else
                        {
                            var _Familiar = new Familiar { FamMicrosoftAdId = FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId };
                            app.NewFamiliar(_Familiar, app);
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            ImageSource.FromFile(@"splash.png");
            app = initData as App;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Familiar = new Familiar();
            oHorarioViewModel = new HorarioViewModel {Visualizar = true, ActivityRunning = false};
            if (authenticated)
            {
                // Hide the Sign-in button.
                oHorarioViewModel.Visualizar = false;
            }
        }
    }
}