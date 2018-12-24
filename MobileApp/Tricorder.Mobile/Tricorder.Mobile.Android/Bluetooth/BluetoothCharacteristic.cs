using System;
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
    }
}