using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RegistrationService.Domain.LicenseSigning
{
    public interface ILicenseSigningManager
    {
        IAsyncEnumerable<SigningProcess> ConsumeAllAsync(CancellationToken cancellationToken);
        Task<SigningProcess> StartAsync(string licenseKey);
        bool TryComplete(string correlationId, string code);
        bool TryFail(string correlationId, string code);
    }
}
