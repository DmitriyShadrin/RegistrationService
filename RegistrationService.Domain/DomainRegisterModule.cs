using Autofac;
using RegistrationService.Domain.Queue;

namespace RegistrationService.Domain
{
    public class DomainRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SigningQueue>().As<ISigningQueue>().SingleInstance();
        }
    }
}
