using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Rhetos.AspNetFormsAuth
{
    public class RhetosUserClaimPrincipalFactory : IUserClaimsPrincipalFactory<IdentityUser>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private IdentityOptions _options;

        public RhetosUserClaimPrincipalFactory(UserManager<IdentityUser> userManager, IOptions<IdentityOptions> optionsAccessor)
        {
            _userManager = userManager;
            _options = optionsAccessor.Value;
        }
        public async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var id = new ClaimsIdentity("Identity.Application", // REVIEW: Used to match Application scheme
                _options.ClaimsIdentity.UserNameClaimType,
                _options.ClaimsIdentity.RoleClaimType);

            id.AddClaim(new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()));
            id.AddClaim(new Claim(_options.ClaimsIdentity.UserNameClaimType, user.UserName));

            if (_userManager.SupportsUserSecurityStamp)
            {
                id.AddClaim(new Claim(_options.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp));
            }

            ClaimsPrincipal principal = new ClaimsPrincipal(id);
            return await Task.FromResult(principal);
        }
    }
}
