using Microsoft.AspNetCore.Identity;
using Rhetos.AspNetFormsAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Rhetos.AspNetFormsAuth
{
    public class RhetosFormAuthenticationService : IRhetosFormAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RhetosFormAuthenticationService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<CommandResult> Login(string username, string password)
        {
            var command = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            return command.Succeeded 
                ? CommandResult.Success() 
                : CommandResult.Failed("Wrong username or password");
        }

        public async Task<CommandResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return CommandResult.Success();
        }

        public async Task<CommandResult> Register(string username, string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = email
            };

            var command = await _userManager.CreateAsync(user, password);

            return command.Succeeded
                ? CommandResult.Success()
                : CommandResult.Failed(command.Errors.First().Description);
        }
    }
}
