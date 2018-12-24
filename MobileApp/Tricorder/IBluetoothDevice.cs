using System.Threading;
using System.Threading.Tasks;

namespace Tricorder
{
    public interface IBluetoothDevice
    {
        string Name { get; }

        Task<IBluetoothService[]> GetServicesAsync();
        Task<IBluetoothService[]> GetServicesAsync(CancellationToken cancellationToken);
    }
}