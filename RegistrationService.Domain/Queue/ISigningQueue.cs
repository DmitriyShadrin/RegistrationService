using RegistrationService.Infrastructure;

namespace RegistrationService.Domain.Queue
{
    public interface ISigningQueue : IQueue<SigningProcess>
    {

    }
}
