using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RegistrationService.Api
{
    public class AuthOptions
    {
        // TODO: secret key vault should be used.
        public const string ISSUER = "RegistrationService";
        public const string AUDIENCE = "LicenseSignGenerator";
        const string KEY = "3E0612D8-6EEF-4888-A586-C3B5E6369B26";
        public const int LIFETIME = 10000;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
