using Android.App;
using Android.Content;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using bt = Android.Bluetooth;

namespace Tricorder.Mobile.Droid.Bluetooth
{
    public class BluetoothDevice : IBluetoothDevice
    {
        public bt.BluetoothDevice PlatformDevice { get; }

        public string Name => PlatformDevice.Name;

        public BluetoothDevice(Android.Bluetooth.BluetoothDevice device)
        {
            PlatformDevice = device;
        }

        public Task<IBluetoothService[]> GetServicesAsync()
        {
            return GetServicesAsync(CancellationToken.None);
        }

        public Task<IBluetoothService[]> GetServicesAsync(CancellationToken cancellationToken)
        {
            TaskCompletionSource<IBluetoothService[]> tcs = new TaskCompletionSource<IBluetoothService[]>();

            DeviceGattCallback gattCallback = new DeviceGattCallback(this.PlatformDevice);

            EventHandler<DeviceGattCallback.ServicesDiscoveredEventArgs> callback = null;

            callback = (o, e) =>
            {
                IBluetoothService[] services = e.Gatt.Services.Select(x => new BluetoothService(x, this)).ToArray();

                tcs.SetResult(services);

                gattCallback.ServicesDiscovered -= callback;
            };

            gattCallback.ServicesDiscovered += callback;

            PlatformDevice.ConnectGatt(Application.Context, true, gattCallback);

            return tcs.Task;
        }
    }
}