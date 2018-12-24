using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tricorder
{
    public interface IBluetoothService
    {
        Guid Id { get; }

        Task<IBluetoothCharacteristic[]> GetCharacteristicsAsync();
        Task<IBluetoothCharacteristic[]> GetCharacteristicsAsync(CancellationToken cancellationToken);
    }
}