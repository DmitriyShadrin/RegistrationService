using Microsoft.AspNetCore.Mvc;
using RegistrationService.Api.Models;
using RegistrationService.Domain;
using RegistrationService.Domain.Queue;
using System.Threading.Tasks;

namespace RegistrationService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ISigningQueue _signingQueue;

        public RegistrationController(ISigningQueue signingQueue)
        {
            _signingQueue = signingQueue;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationForm form)
        {
            if (!TryValidateModel(form, nameof(form)))
            {
                return BadRequest();
            }

            var signingProcess = new SigningProcess();
            await _signingQueue.AddAsync(signingProcess);
            var result = await signingProcess.InvokeAsync();

            return Ok();
        }
    }
}
