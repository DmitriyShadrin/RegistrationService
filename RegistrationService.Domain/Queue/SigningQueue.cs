using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RegistrationService.Domain.Queue
{
    public class SigningQueue : ISigningQueue
    {
        private readonly Channel<SigningProcess> _items;

        public SigningQueue()
        {
            _items = Channel.CreateUnbounded<SigningProcess>();
        }

        public async Task AddAsync(SigningProcess item)
        {
            await _items.Writer.WriteAsync(item);
        }

        public IAsyncEnumerable<SigningProcess> ReadAsync(CancellationToken cancelationToken)
        {
            return _items.Reader.ReadAllAsync(cancelationToken);
        }
    }
}
