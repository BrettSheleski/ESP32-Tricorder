using Android.Runtime;
using System;
using bt = Android.Bluetooth;

namespace Tricorder.Mobile.Droid.Bluetooth
{
    class DeviceGattCallback : bt.BluetoothGattCallback
    {
        private bt.BluetoothDevice platformDevice;

        public DeviceGattCallback(bt.BluetoothDevice platformDevice)
        {
            this.platformDevice = platformDevice;
        }

        public event EventHandler<ServicesDiscoveredEventArgs> ServicesDiscovered;
        public event EventHandler<ConnectionStateChangeEventArgs> ConnectionStateChange;
        public event EventHandler<CharacteristicChangedEventArgs> CharacteristicChanged;
        public event EventHandler<CharacteristicValueChangedEventArgs> CharacteristicWrite;
        public event EventHandler<CharacteristicValueChangedEventArgs> CharacteristicRead;
        
        public override void OnServicesDiscovered(bt.BluetoothGatt gatt, [GeneratedEnum] bt.GattStatus status)
        {
            base.OnServicesDiscovered(gatt, status);

            ServicesDiscovered?.Invoke(this, new ServicesDiscoveredEventArgs(gatt, status));
        }

        

        public override void OnConnectionStateChange(bt.BluetoothGatt gatt, [GeneratedEnum] bt.GattStatus status, [GeneratedEnum] bt.ProfileState newState)
        {
            base.OnConnectionStateChange(gatt, status, newState);

            ConnectionStateChange?.Invoke(this, new ConnectionStateChangeEventArgs(gatt, status, newState));

            //gatt.DiscoverServices();
        }

        public override void OnCharacteristicChanged(bt.BluetoothGatt gatt, bt.BluetoothGattCharacteristic characteristic)
        {
            base.OnCharacteristicChanged(gatt, characteristic);

            CharacteristicChanged?.Invoke(this, new CharacteristicChangedEventArgs(gatt, characteristic));
        }

        public override void OnCharacteristicWrite(bt.BluetoothGatt gatt, bt.BluetoothGattCharacteristic characteristic, [GeneratedEnum] bt.GattStatus status)
        {
            base.OnCharacteristicWrite(gatt, characteristic, status);

            this.CharacteristicWrite?.Invoke(this, new CharacteristicValueChangedEventArgs(gatt, characteristic, status));
        }

        public override void OnCharacteristicRead(bt.BluetoothGatt gatt, bt.BluetoothGattCharacteristic characteristic, [GeneratedEnum] bt.GattStatus status)
        {
            base.OnCharacteristicRead(gatt, characteristic, status);

            this.CharacteristicRead?.Invoke(this, new CharacteristicValueChangedEventArgs(gatt, characteristic, status));
        }


        public class ConnectionStateChangeEventArgs : EventArgs
        {
            public ConnectionStateChangeEventArgs(bt.BluetoothGatt gatt, bt.GattStatus status, bt.ProfileState newState)
            {
                Gatt = gatt;
                Status = status;
                NewState = newState;
            }

            public bt.BluetoothGatt Gatt { get; set; }
            public bt.GattStatus Status { get; set; }
            public bt.ProfileState NewState { get; set; }
        }


        public class CharacteristicValueChangedEventArgs : EventArgs
        {
            public bt.BluetoothGatt Gatt { get; }
            public bt.BluetoothGattCharacteristic Characteristic { get; }
            public bt.GattStatus Status { get; }

            public CharacteristicValueChangedEventArgs(bt.BluetoothGatt gatt, bt.BluetoothGattCharacteristic characteristic, bt.GattStatus status)
            {
                this.Gatt = gatt;
                this.Characteristic = characteristic;
                this.Status = status;
            }
        }

        public class CharacteristicChangedEventArgs : EventArgs
        {
            public bt.BluetoothGatt Gatt { get; }
            public bt.BluetoothGattCharacteristic Characteristic { get; }

            public CharacteristicChangedEventArgs(bt.BluetoothGatt gatt, bt.BluetoothGattCharacteristic characteristic)
            {
                this.Gatt = gatt;
                this.Characteristic = characteristic;
            }
        }
        public class ServicesDiscoveredEventArgs : EventArgs
        {
            public ServicesDiscoveredEventArgs(bt.BluetoothGatt gatt, bt.GattStatus status)
            {
                Gatt = gatt;
                Status = status;
            }

            public bt.BluetoothGatt Gatt { get; }
            public bt.GattStatus Status { get; }
        }
    }
}