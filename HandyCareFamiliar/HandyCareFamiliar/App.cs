using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Interface;
using HandyCareFamiliar.Model;
using HandyCareFamiliar.PageModel;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace HandyCareFamiliar
{
    public class App : Application
    {
        public static bool Authenticated;

        public App()
        {
            Register();
            var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>(this);
            var mainPage = new FreshNavigationContainer(page);
            MainPage = mainPage;
        }

        public static ObservableCollection<Afazer> Afazeres { get; set; }
        public static ObservableCollection<ConclusaoAfazer> AfazeresConcluidos { get; set; }

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        private void Register()
        {
            FreshIOC.Container.Register<IFamiliarRestService, FamiliarRestService>();
        }

        public static async Task GetAfazeres(bool sync)
        {
            try
            {
                await Task.Run(async () =>
                {
                    AfazeresConcluidos =
                        new ObservableCollection<ConclusaoAfazer>(
                            await FamiliarRestService.DefaultManager.RefreshConclusaoAfazerAsync(sync));

                    var selection =
                        new ObservableCollection<Afazer>(
                            await FamiliarRestService.DefaultManager.RefreshAfazerAsync(sync));
                    if ((selection.Count > 0) && (AfazeresConcluidos.Count > 0))
                    {
                        var pacresult =
                            new ObservableCollection<CuidadorPaciente>(
                                    await FamiliarRestService.DefaultManager.RefreshCuidadorPacienteAsync(sync))
                                .AsEnumerable();
                        var result = selection.Where(e => !AfazeresConcluidos.Select(m => m.ConAfazer)
                                .Contains(e.Id))
                            .Where(e => pacresult.Select(m => m.Id).Contains(e.AfaPaciente))
                            .AsEnumerable();
                        Afazeres = new ObservableCollection<Afazer>(result);
                    }
                    else
                    {
                        Afazeres = new ObservableCollection<Afazer>(selection);
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AbrirMainMenu(Familiar familiar)
        {
            var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel>(familiar);
            var mainPage = new FreshNavigationContainer(page);
            MainPage = mainPage;
        }

        public void NewFamiliar(Familiar Familiar, App app)
        {
            var x = new Tuple<Familiar, App>(Familiar, app);
            var page = FreshPageModelResolver.ResolvePageModel<FamiliarPageModel>(x);
            var mainPage = new FreshNavigationContainer(page);
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override async void OnSleep()
        {
            if (Authenticated)
                await Task.Run(() =>
                {
                    if (FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId != null)
                        Current.Properties["UserId"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                    if (FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.MobileServiceAuthenticationToken !=
                        null)
                        Current.Properties["Token"] =
                            FamiliarRestService.DefaultManager.CurrentClient.CurrentUser
                                .MobileServiceAuthenticationToken;
                    Debug.WriteLine("OnSleeping");
                });
        }

        protected override async void OnResume()
        {
            if (Authenticated)
                await Task.Run(() =>
                {
                    if (Properties.ContainsKey("UserID"))
                        FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.UserId =
                            (string) Properties["UserId"];
                    if (Properties.ContainsKey("Token"))
                        FamiliarRestService.DefaultManager.CurrentClient.CurrentUser.MobileServiceAuthenticationToken =
                            (string) Properties["Token"];
                    Debug.WriteLine("OnResuming");
                });
        }

        public interface IAuthenticate
        {
            Task<bool> Authenticate(MobileServiceAuthenticationProvider provider);
        }
    }
}