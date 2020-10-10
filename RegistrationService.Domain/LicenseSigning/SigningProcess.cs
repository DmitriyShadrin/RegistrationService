using System.Threading.Tasks;

namespace RegistrationService.Domain.LicenseSigning
{
    public class SigningProcess
    {
        private TaskCompletionSource<SigningResult> _completionSource;

        public string CorrelationId { get; }
        public string LicenseKey { get; }

        public SigningProcess(string correlationId, string licenseKey)
        {
            _completionSource = new TaskCompletionSource<SigningResult>();
            CorrelationId = correlationId;
            LicenseKey = licenseKey;
        }

        public Task<SigningResult> InvokeAsync()
        {
            return _completionSource.Task;
        }

        public void Finish(bool v, string code)
        {
            _completionSource.TrySetResult(new SigningResult());
        }
    }
}
