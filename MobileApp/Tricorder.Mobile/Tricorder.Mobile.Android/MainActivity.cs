using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Tricorder.Mobile.Droid
{
    [Activity(Label = "Tricorder", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                // Kill status bar underlay added by FormsAppCompatActivity
                // Must be done before calling FormsAppCompatActivity.OnCreate()
                var statusBarHeightInfo = typeof(Xamarin.Forms.Platform.Android.FormsAppCompatActivity).GetField("statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (statusBarHeightInfo == null)
                {
                    statusBarHeightInfo = typeof(Xamarin.Forms.Platform.Android.FormsAppCompatActivity).GetField("_statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                }
                statusBarHeightInfo?.SetValue(this, 0);
            }

            base.OnCreate(bundle);

            if (IsEmulator())
            {
                Xamarin.Forms.DependencyService.Register<IBluetoothManager, Fakes.FakeBluetoothManager>();

                Fakes.FakeBluetoothService service;
                Fakes.FakeBluetoothDevice device;
                for (int i = 0; i < 10; ++i)
                {
                    device = new Fakes.FakeBluetoothDevice()
                    {
                        Name = $"Fake Device #{i + 1}",

                    };

                    Fakes.FakeBluetoothManager.Devices.Add(device);

                    for (int j = 0; j < 5; ++j)
                    {
                        service = new Fakes.FakeBluetoothService
                        {
                            Id = Guid.NewGuid()
                        };

                        for (int k = 0; k < 5; ++k)
                        {
                            service.Characteristics.Add(new Fakes.FakeBluetoothCharacteristic
                            {
                                Id = Guid.NewGuid()
                            });
                        }

                        device.Services.Add(service);
                    }

                }

            }
            else
            {
                Xamarin.Forms.DependencyService.Register<IBluetoothManager, Bluetooth.TricorderBluetoothManager>();

            }


            Bluetooth.TricorderBluetoothManager.Activity = this;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            
            this.Window.AddFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.TurnScreenOn);

            LoadApplication(new App());

        }

        public static bool IsEmulator()
        {
            return Build.Fingerprint.StartsWith("generic")
                    || Build.Fingerprint.StartsWith("unknown")
                    || Build.Model.Contains("google_sdk")
                    || Build.Model.Contains("Emulator")
                    || Build.Model.Contains("Android SDK built for x86")
                    || Build.Manufacturer.Contains("Genymotion")
                    || (Build.Brand.StartsWith("generic") && Build.Device.StartsWith("generic"))
                    || "google_sdk".Equals(Build.Product);
        }
    }
}

