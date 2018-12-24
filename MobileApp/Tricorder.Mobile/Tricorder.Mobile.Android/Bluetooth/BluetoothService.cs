using System;
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
        }

        public Guid Id { get; }

        public bt.BluetoothGattService GattService { get; }
        internal BluetoothDevice Device { get; }

        public Task<IBluetoothCharacteristic[]> GetCharacteristicsAsync()
        {
            return GetCharacteristicsAsync(CancellationToken.None);
        }

        public Task<IBluetoothCharacteristic[]> GetCharacteristicsAsync(CancellationToken cancellationToken)
        {
             return Task.FromResult(GattService.Characteristics.Select(x => (IBluetoothCharacteristic)new BluetoothCharacteristic(x, this)).ToArray());
        }
    }
}