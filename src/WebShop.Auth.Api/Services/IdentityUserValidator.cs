using WebShop.Users.Data.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebShop.Auth.Api.Services
{
    public class IdentityUserValidator : IUserValidator
    {
        readonly UserManager<ApplicationUser> _userManager;      

        public IdentityUserValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> AutoProvisionUserAsync(string provider, string userId, IEnumerable<Claim> claims)
        {
           var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email, StringComparison.CurrentCultureIgnoreCase))?.Value,
                UserName = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email, StringComparison.CurrentCultureIgnoreCase))?.Value,
                FirstName = claims.FirstOrDefault(c=>c.Type.Equals(ClaimTypes.GivenName,StringComparison.CurrentCultureIgnoreCase))?.Value,
                LastName = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Surname, StringComparison.CurrentCultureIgnoreCase))?.Value
           };
            await _userManager.CreateAsync(user, Guid.NewGuid().ToString().ToSha512());
        
            return user;
        }

        public async Task<(ApplicationUser user, string provider, string providerUserId, IEnumerable<Claim> claims)> FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            //var providerUserId = userIdClaim.Value;
            var providerUserId = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email, StringComparison.CurrentCultureIgnoreCase))?.Value;

            // find external user
            //var user = _users.FindByExternalProvider(provider, providerUserId);
            var user = await FindByUsernameAsync(providerUserId);
            return (user, provider, providerUserId, claims);
        }

        public async Task<ApplicationUser> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByEmailAsync(username);
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            if (user != null)
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }
            return false;
        }
    }
}
