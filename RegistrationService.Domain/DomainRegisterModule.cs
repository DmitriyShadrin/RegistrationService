using Autofac;
using RegistrationService.Domain.LicenseSigning;

namespace RegistrationService.Domain
{
    public class DomainRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LicenseSigningManager>().As<ILicenseSigningManager>().SingleInstance();
        }
    }
}
