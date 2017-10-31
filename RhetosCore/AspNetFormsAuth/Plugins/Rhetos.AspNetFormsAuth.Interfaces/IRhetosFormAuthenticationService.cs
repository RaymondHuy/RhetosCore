using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rhetos.AspNetFormsAuth.Interfaces
{
    public interface IRhetosFormAuthenticationService
    {
        Task<CommandResult> Login(string username, string password);

        Task<CommandResult> Register(string username, string email, string password);

        Task<CommandResult> Logout();
    }
}
