using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tricorder
{
    public interface IBluetoothCharacteristic
    {
        Guid Id { get;  }

        Task<byte[]> GetValueAsync();
        Task<byte[]> GetValueAsync(CancellationToken cancellationToken);

        Task SetValueAsync(byte[] value);
        Task SetValueAsync(byte[] value, CancellationToken cancellationToken);
    }
}