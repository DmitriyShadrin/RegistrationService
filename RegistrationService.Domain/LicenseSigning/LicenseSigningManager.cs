using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;

namespace RegistrationService.Domain.LicenseSigning
{
    public class LicenseSigningManager : ILicenseSigningManager
    {
        // TODO: Should be used distributed cache, e.g. Redis. 
        private static readonly ConcurrentDictionary<string, SigningProcess> _cache = new ConcurrentDictionary<string, SigningProcess>();
        private readonly Channel<SigningProcess> _items;
        private readonly ILogger<LicenseSigningManager> _logger;

        public LicenseSigningManager(ILogger<LicenseSigningManager> logger)
        {
            _logger = logger;
            _items = Channel.CreateUnbounded<SigningProcess>();
        }

        public IAsyncEnumerable<SigningProcess> ConsumeAllAsync(CancellationToken cancellationToken)
        {
            return _items.Reader.ReadAllAsync(cancellationToken);
        }

        public SigningProcess Start(string licenseKey)
        {
            var correlationId = Guid.NewGuid().ToString();
            var process = new SigningProcess(correlationId, licenseKey);

            if (!_cache.TryAdd(correlationId, process))
            {
                _logger.LogWarning($"Signing process with correlation id {correlationId} could not be stored in the cache.");
            }

            return process;
        }

        public bool TryComplete(string correlationId, string code)
        {
            return TryFinish(correlationId, true, code);
        }

        public bool TryFail(string correlationId, string code)
        {
            return TryFinish(correlationId, false, code);
        }

        private bool TryFinish(string correlationId, bool state, string code)
        {
            if (!_cache.TryRemove(correlationId, out var signingProcess))
            {
                _logger.LogWarning($"Signing process with correlation id {correlationId} was not found int the cache.");
                return false;
            }

            signingProcess.Finish(state, code);
            return true;
        }
    }
}
