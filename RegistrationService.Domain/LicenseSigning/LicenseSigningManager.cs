﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

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

        public IAsyncEnumerable<SigningProcess> ConsumeAllAsync(CancellationToken cancellationToken = default)
        {
            return _items.Reader.ReadAllAsync(cancellationToken);
        }

        public async Task<SigningProcess> StartAsync(string licenseKey, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(licenseKey))
            {
                throw new ArgumentException($"'{nameof(licenseKey)}' cannot be null or whitespace", nameof(licenseKey));
            }

            var correlationId = Guid.NewGuid().ToString();
            var process = new SigningProcess(correlationId, licenseKey);

            if (!_cache.TryAdd(correlationId, process))
            {
                _logger.LogWarning($"Signing process with correlation id {correlationId} could not be stored in the cache.");
            }

            await _items.Writer.WriteAsync(process);

            return process;
        }

        public bool TryComplete(string correlationId, string code)
        {
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                throw new ArgumentException($"'{nameof(correlationId)}' cannot be null or whitespace", nameof(correlationId));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"'{nameof(code)}' cannot be null or whitespace", nameof(code));
            }

            return TryFinish(correlationId, true, code);
        }

        public bool TryFail(string correlationId, string code)
        {
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                throw new ArgumentException($"'{nameof(correlationId)}' cannot be null or whitespace", nameof(correlationId));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"'{nameof(code)}' cannot be null or whitespace", nameof(code));
            }

            return TryFinish(correlationId, false, code);
        }

        private bool TryFinish(string correlationId, bool state, string code)
        {
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                throw new ArgumentException($"'{nameof(correlationId)}' cannot be null or whitespace", nameof(correlationId));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"'{nameof(code)}' cannot be null or whitespace", nameof(code));
            }

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
