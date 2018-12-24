using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using bt = Android.Bluetooth;

namespace Tricorder.Mobile.Droid.Bluetooth
{
    public class TricorderBluetoothManager : IBluetoothManager
    {
        public static Activity Activity { get; set; }

        public TricorderBluetoothManager() : this((bt.BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService))
        {

        }

        public TricorderBluetoothManager(bt.BluetoothManager bluetoothManager)
        {
            this.PlatformManager = bluetoothManager;
        }

        public bt.BluetoothManager PlatformManager { get; }

        public Task<IBluetoothDevice[]> GetDevicesAsync()
        {
            return GetDevicesAsync(CancellationToken.None);
        }

        public Task<IBluetoothDevice[]> GetDevicesAsync(CancellationToken cancellationToken)
        {
            List<IBluetoothDevice> devices = new List<IBluetoothDevice>();

            if (PlatformManager.Adapter != null)
            {
                foreach (var device in PlatformManager.Adapter.BondedDevices)
                {
                    devices.Add(new BluetoothDevice(device));
                }
            }

            return Task.FromResult(devices.ToArray());
        }
    }
}