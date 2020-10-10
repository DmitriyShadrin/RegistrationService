using Autofac;
using Microsoft.Extensions.Configuration;
using RegistrationService.Domain;

namespace RegistrationService.Api
{
    public class ApiRegisterModule : Module
    {
        private IConfiguration _config;

        public ApiRegisterModule(IConfiguration config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new DomainRegisterModule());
        }
    }
}
