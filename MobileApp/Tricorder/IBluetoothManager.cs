using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tricorder
{
    public interface IBluetoothManager
    {
        Task<IBluetoothDevice[]> GetDevicesAsync();
        Task<IBluetoothDevice[]> GetDevicesAsync(CancellationToken cancellationToken);
        
    }
}
