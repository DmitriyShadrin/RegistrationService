using Grpc.Core;
using RegistrationService.Domain.LicenseSigning;
using System.Threading.Tasks;

namespace RegistrationService.Api.Services
{
    public class SigningService : SignLicense.SignLicenseBase
    {
        private readonly ILicenseSigningManager _licenseSigningManager;

        public SigningService(ILicenseSigningManager licenseSigningManager)
        {
            _licenseSigningManager = licenseSigningManager;
            //_logger = logger;
        }

        public override async Task ReadLicenses(ReadLicenseRequest request, IServerStreamWriter<ReadLicenseResponse> responseStream, ServerCallContext context)
        {
            await foreach (var item in _licenseSigningManager.ConsumeAllAsync(context.CancellationToken))
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await responseStream.WriteAsync(new ReadLicenseResponse { LicenseKey = item.LicenseKey, CorrelationId = item.CorrelationId });
            }
        }

        public override Task<SignedResponse> SendSignedLicense(SignedRequest request, ServerCallContext context)
        {
            var code = Convert(request.Code);

            bool state;
            if (request.Code == SigningCode.Ok)
            {
                state = _licenseSigningManager.TryComplete(request.CorrelationId, code);
            }
            else
            {
                state = _licenseSigningManager.TryFail(request.CorrelationId, code);
            }

            return Task.FromResult(new SignedResponse { State = state });
        }

        private string Convert(SigningCode code)
        {
            return code switch
            {
                SigningCode.Invalid => "LICENSE_IS_INVALID",
                SigningCode.Error => "SIGNING_FAILED",
                SigningCode.Ok => "OK",
                _ => "UNKNOWN"
            };
        }
    }
}
