using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Com.Syncfusion.Rating;
using Gcm.Client;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Droid.Services;
using Java.IO;
using Java.Net;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Com.Syncfusion.Rating;
using Octane.Xam.VideoPlayer.Android;
using Syncfusion.SfRating.XForms.Droid;
using Xamarin.Forms.Platform.Android;
using Debug = System.Diagnostics.Debug;
using Environment = Android.OS.Environment;
using Exception = Java.Lang.Exception;


namespace HandyCareFamiliar.Droid
{
    [Activity(Label = "Handy Care - Familiar", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light",
         MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity, App.IAuthenticate
    {
        private static readonly File FotoFile = new File(Environment.GetExternalStoragePublicDirectory(
            Environment.DirectoryPictures), DateTime.Now + "handycare.jpg");

        private static readonly File VideoFile = new File(Environment.GetExternalStoragePublicDirectory(
            Environment.DirectoryMovies), DateTime.Now + "handycare.mp4");

        private static AfazerServiceConnection _afazerServiceConnection;

        // declarations
        protected readonly string logTag = "App";
        private MobileServiceUser _user;

        // Return the current activity instance.
        public static MainActivity CurrentActivity { get; private set; }

        public AfazerService AfazerService
        {
            get
            {
                if (_afazerServiceConnection.Binder == null)
                    throw new Exception("Fodeu");
                return _afazerServiceConnection.Binder.Service;
            }
        }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                _user = await FamiliarRestService.DefaultManager.CurrentClient.LoginAsync(this,
                    provider);
                if (_user != null)
                {
                    message = $"you are now signed-in as {_user.UserId}.";
                    success = true;
                    var a = new Thread(() => { ThreadPool.QueueUserWorkItem(async o => await StartAfazerService()); });
                    a.Start();
                }
            }
            catch (System.Exception ex)
            {
                message = ex.Message;
            }
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();
            return success;
        }

        public event EventHandler<ServiceConnectedEventArgs> AfazerServiceConnected = delegate { };

        private async Task StartAfazerService()
        {
            _afazerServiceConnection = new AfazerServiceConnection(null);
            _afazerServiceConnection.ServiceConnected += (sender, e) => { AfazerServiceConnected(this, e); };
            await Task.Run(() =>
            {
                StartService(new Intent(this, typeof(AfazerService)));
                var afazerServiceIntent = new Intent(this, typeof(AfazerService));
                BindService(afazerServiceIntent, _afazerServiceConnection, Bind.AutoCreate);
            });
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            CurrentActivity = this;
            Forms.Init(this, bundle);
            CurrentPlatform.Init();
            App.Init(this);
            new SfRatingRenderer();
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            UserDialogs.Init(() => this);
            FormsVideoPlayer.Init();
            LoadApplication(new App());
            try
            {
                // Check to ensure everything's setup right
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                // Register for push notifications
                Debug.WriteLine("Registering...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            }
            catch (System.Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }
        }

        private void CreateAndShowDialog(string message, string title)
        {
            var builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            var app = Xamarin.Forms.Application.Current as App;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}