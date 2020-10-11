using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RegistrationService.Domain.LicenseSigning
{
    public interface ILicenseSigningManager
    {
        IAsyncEnumerable<SigningProcess> ConsumeAllAsync(CancellationToken cancellationToken = default);
        Task<SigningProcess> StartAsync(string licenseKey, CancellationToken cancellationToken = default);
        bool TryComplete(string correlationId, string code);
        bool TryFail(string correlationId, string code);
    }
}
