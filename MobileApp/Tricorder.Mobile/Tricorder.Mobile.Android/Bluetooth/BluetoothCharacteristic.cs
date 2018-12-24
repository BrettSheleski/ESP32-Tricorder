using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;

namespace Tricorder.Mobile.Droid.Bluetooth
{
    public class BluetoothCharacteristic : IBluetoothCharacteristic
    {
        private BluetoothGattCharacteristic GattCharacteristic;
        public BluetoothService Service { get; }

        public BluetoothCharacteristic(BluetoothGattCharacteristic gattCharacteristic, BluetoothService service)
        {
            this.Service = service;
            this.GattCharacteristic = gattCharacteristic;
            
            this.Id = new Guid(gattCharacteristic.Uuid.ToString());
        }

        public Guid Id { get; }
        public TaskCompletionSource<byte[]> GetValueCompletionSource { get; set; }

        public Task<byte[]> GetValueAsync()
        {
            return GetValueAsync(CancellationToken.None);
        }

        public Task<byte[]> GetValueAsync(CancellationToken cancellationToken)
        {
            if (GetValueCompletionSource == null)
            {
                this.GetValueCompletionSource = new TaskCompletionSource<byte[]>();
                
                Service.Device.Gatt.ReadCharacteristic(this.GattCharacteristic);
            }
            
            return GetValueCompletionSource.Task;
        }

        public Task SetValueAsync(byte[] value)
        {
            return SetValueAsync(value, CancellationToken.None);
        }

        public Task SetValueAsync(byte[] value, CancellationToken cancellationToken)
        {
            this.GattCharacteristic.SetValue(value);

            return Task.CompletedTask;
        }
    }
}