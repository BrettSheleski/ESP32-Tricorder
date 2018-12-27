using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tricorder.Mobile.Fakes
{
    public class FakeBluetoothManager : IBluetoothManager
    {
        public static List<FakeBluetoothDevice> Devices { get; } = new List<FakeBluetoothDevice>();

        async Task<IBluetoothDevice[]> IBluetoothManager.GetDevicesAsync()
        {
            return await Task.Run(() => Devices.ToArray());
        }

        async Task<IBluetoothDevice[]> IBluetoothManager.GetDevicesAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => Devices.ToArray());
        }
    }

    public class FakeBluetoothDevice : IBluetoothDevice
    {
        public string Name { get; set; }

        public List<FakeBluetoothService> Services { get; } = new List<FakeBluetoothService>();

        async Task<IBluetoothService[]> IBluetoothDevice.GetServicesAsync()
        {
            return await Task.Run(() => Services.ToArray());
        }

        async Task<IBluetoothService[]> IBluetoothDevice.GetServicesAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => Services.ToArray());
        }
    }

    public class FakeBluetoothService : IBluetoothService
    {
        public Guid Id { get; set; }

        public List<FakeBluetoothCharacteristic> Characteristics { get; } = new List<FakeBluetoothCharacteristic>();

        async Task<IBluetoothCharacteristic[]> IBluetoothService.GetCharacteristicsAsync()
        {
           return await Task.Run(() => Characteristics.ToArray());
        }

        async Task<IBluetoothCharacteristic[]> IBluetoothService.GetCharacteristicsAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => Characteristics.ToArray());
        }
    }

    public class FakeBluetoothCharacteristic : IBluetoothCharacteristic
    {
        public Guid Id { get; set; }
        
        private static byte[] GetRandomInt()
        {
            Random r = new Random((int)DateTime.Now.Ticks);

            int i = r.Next(1000);

            return BitConverter.GetBytes(i);
        }

        Task<byte[]> IBluetoothCharacteristic.GetValueAsync()
        {
            return Task.FromResult(GetRandomInt());
        }

        Task<byte[]> IBluetoothCharacteristic.GetValueAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetRandomInt());
        }

        Task IBluetoothCharacteristic.SetValueAsync(byte[] value)
        {
            throw new NotSupportedException();
        }

        Task IBluetoothCharacteristic.SetValueAsync(byte[] value, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}
