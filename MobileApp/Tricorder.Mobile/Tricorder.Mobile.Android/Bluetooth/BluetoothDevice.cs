using Android.App;
using Android.Content;
using Android.Runtime;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using bt = Android.Bluetooth;

namespace Tricorder.Mobile.Droid.Bluetooth
{
    public  class BluetoothDevice : bt.BluetoothGattCallback, IBluetoothDevice
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

        public void Connect()
        {
            if (!IsConnected)
            {
                this.Gatt = PlatformDevice.ConnectGatt(Application.Context, true, this);
                IsConnected = true;
            }
        }

        public bt.BluetoothGatt Gatt { get; private set; }
        public bool IsConnected { get; private set; }

        private bool _servicesDiscovered = false;

        public Dictionary<UUID, BluetoothService> ServicesById { get; } = new Dictionary<UUID, BluetoothService>();

        public Task<IBluetoothService[]> GetServicesAsync(CancellationToken cancellationToken)
        {
            if (_servicesDiscovered)
            {
                return Task.FromResult(ServicesById.Values.Select(x => (IBluetoothService)x).ToArray());
            }
            else
            {
                TaskCompletionSource<IBluetoothService[]> tcs = new TaskCompletionSource<IBluetoothService[]>();

                EventHandler<ServicesDiscoveredEventArgs> eventHandler = null;

                eventHandler = (o, e) =>
                {
                    BluetoothService service;

                    List<IBluetoothService> servicesList = new List<IBluetoothService>();
                    foreach(var platformService in e.Gatt.Services)
                    {
                        service = new BluetoothService(platformService, this);

                        this.ServicesById[platformService.Uuid] = service;
                        servicesList.Add(service);
                    }
                    
                    this.ServicesDiscovered -= eventHandler;

                    tcs.SetResult(servicesList.ToArray());
                };

                this.ServicesDiscovered += eventHandler;

                Connect();
                
                return tcs.Task;
            }
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
            
            _servicesDiscovered = true;
        }
        
        public override void OnConnectionStateChange(bt.BluetoothGatt gatt, [GeneratedEnum] bt.GattStatus status, [GeneratedEnum] bt.ProfileState newState)
        {
            base.OnConnectionStateChange(gatt, status, newState);

            ConnectionStateChange?.Invoke(this, new ConnectionStateChangeEventArgs(gatt, status, newState));

            if (!_servicesDiscovered)
            {
                gatt.DiscoverServices();
            }
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
            
            byte[] value = characteristic.GetValue();

            var myChar = this.ServicesById[characteristic.Service.Uuid].CharacteristicsById[characteristic.Uuid];

            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);

            myChar.GetValueCompletionSource?.SetResult(value);
            myChar.GetValueCompletionSource = null;

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