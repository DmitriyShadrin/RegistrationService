using System.ComponentModel.DataAnnotations;

namespace RegistrationService.Api.Models
{
    public class RegistrationForm
    {
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        [Required]
        public string LicenseKey { get; set; }
    }
}
