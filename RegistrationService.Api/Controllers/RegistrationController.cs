﻿using Microsoft.AspNetCore.Mvc;
using RegistrationService.Api.Models;
using RegistrationService.Domain.LicenseSigning;
using System.Threading.Tasks;

namespace RegistrationService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ILicenseSigningManager _licenseSigningManager;

        public RegistrationController(ILicenseSigningManager licenseSigningManager)
        {
            _licenseSigningManager = licenseSigningManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationForm form)
        {
            if (!TryValidateModel(form, nameof(form)))
            {
                return BadRequest();
            }

            var signingProcess = await _licenseSigningManager.StartAsync(form.LicenseKey);
            var result = await signingProcess.InvokeAsync();

            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Code);
            }
        }
    }
}
