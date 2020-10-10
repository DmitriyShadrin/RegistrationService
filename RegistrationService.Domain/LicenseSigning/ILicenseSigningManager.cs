using System.Collections.Generic;
using System.Threading;

namespace RegistrationService.Domain.LicenseSigning
{
    public interface ILicenseSigningManager
    {
        IAsyncEnumerable<SigningProcess> ConsumeAllAsync(CancellationToken cancellationToken);
        SigningProcess Start(string licenseKey);
        bool TryComplete(string correlationId, string code);
        bool TryFail(string correlationId, string code);
    }
}
