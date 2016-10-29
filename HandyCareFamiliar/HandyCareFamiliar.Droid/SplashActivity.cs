using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace HandyCareFamiliar.Droid
{
    //, ScreenOrientation = ScreenOrientation.Portrait
    [Activity(Label = "Handy Care", Theme = "@style/Theme.Splash" , MainLauncher = true, NoHistory = true,
         ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //[Activity(Label = "Handy Care", Theme = "@style/Theme.Splash" /*"@android:style/Theme.Holo.Light.NoActionBar.Fullscreen"*/, MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashLayout);
            ThreadPool.QueueUserWorkItem(async o => await StartApp());
        }

        private async Task StartApp()
        {
            await Task.Run(() =>
            {
                var watch = Stopwatch.StartNew();
                RunOnUiThread(() => { StartActivity(new Intent(this, typeof(MainActivity))); });
                watch.Stop();
                Thread.Sleep(Convert.ToInt32(watch.ElapsedMilliseconds));
            });
        }
    }
}