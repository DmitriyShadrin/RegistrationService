using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RegistrationService.Infrastructure
{
    public interface IQueue<T>
    {
        IAsyncEnumerable<T> ReadAsync(CancellationToken cancelationToken);
        Task AddAsync(T item);
    }
}
