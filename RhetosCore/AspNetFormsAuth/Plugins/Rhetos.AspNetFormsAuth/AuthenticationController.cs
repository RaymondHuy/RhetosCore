using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Logging;
using Rhetos.Security;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Rhetos.AspNetFormsAuth.ViewModels;
using Rhetos.AspNetFormsAuth.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Rhetos.AspNetFormsAuth
{
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IRhetosFormAuthenticationService _rhetosFormAuthenticationService;

        public AuthenticationController(
            IRhetosFormAuthenticationService rhetosFormAuthenticationService)
        {
            _rhetosFormAuthenticationService = rhetosFormAuthenticationService;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel register)
        {
            var command = await _rhetosFormAuthenticationService.Register(register.UserName, register.Email, register.Password);

            if (command.IsSuccess)
                return Ok();
            else return BadRequest(command.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel login)
        {
            var command = await _rhetosFormAuthenticationService.Login(login.UserName, login.Password);

            if (command.IsSuccess)
                return Ok();
            else return BadRequest(command.Message);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var command = await _rhetosFormAuthenticationService.Logout();

            if (command.IsSuccess)
                return Ok();
            else return BadRequest(command.Message);
        }
    }
}
