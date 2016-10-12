using System;
using Android.OS;

namespace HandyCareFamiliar.Droid.Services
{
    public class ServiceConnectedEventArgs : EventArgs
    {
        public IBinder Binder { get; set; }
    }
}