using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using bt = Android.Bluetooth;

namespace Tricorder.Mobile.Droid.Bluetooth
{
    public class BluetoothService : IBluetoothService
    {
        public BluetoothService(bt.BluetoothGattService gattService, BluetoothDevice device)
        {
            this.GattService = gattService;
            this.Device = device;

            Guid id = new Guid(GattService.Uuid.ToString());

            this.Id = id;

            BluetoothCharacteristic characteristic;
            foreach (var gattCharacteristic in gattService.Characteristics)
            {
                characteristic = new BluetoothCharacteristic(gattCharacteristic, this);

                CharacteristicsById[gattCharacteristic.Uuid] = characteristic;
            }
        }

        public Guid Id { get; }

        public bt.BluetoothGattService GattService { get; }
        internal BluetoothDevice Device { get; }

        public Task<IBluetoothCharacteristic[]> GetCharacteristicsAsync()
        {
            return GetCharacteristicsAsync(CancellationToken.None);
        }

        public Dictionary<UUID, BluetoothCharacteristic> CharacteristicsById { get; } = new Dictionary<UUID, BluetoothCharacteristic>();

        public Task<IBluetoothCharacteristic[]> GetCharacteristicsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.CharacteristicsById.Values.Select(x => (IBluetoothCharacteristic)x).ToArray());
        }
    }
}