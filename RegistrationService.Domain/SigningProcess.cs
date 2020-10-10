using System.Threading.Tasks;

namespace RegistrationService.Domain
{
    public class SigningProcess
    {
        private TaskCompletionSource<SigningResult> _completionSource;

        public SigningProcess()
        {
            _completionSource = new TaskCompletionSource<SigningResult>();
        }

        public Task<SigningResult> InvokeAsync()
        {
            return _completionSource.Task;
        }

        public void Finish()
        {
            _completionSource.TrySetResult(new SigningResult());
        }
    }
}
