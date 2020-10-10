using Microsoft.AspNetCore.Mvc;
using RegistrationService.Api.Models;
using System.Threading.Tasks;

namespace RegistrationService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationForm form)
        {
            return Ok();
        }
    }
}
