using WebShop.Users.Data.Entities;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebShop.Auth.Api.Services
{
    public class IdentityProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;
        protected RoleManager<IdentityRole> _roleManager;

        public IdentityProfileService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim("userid", user.Id),
                new Claim("email", user.Email)
            };
            foreach (var roleName in await _userManager.GetRolesAsync(user))
            {
                claims = claims.Concat(await _roleManager.GetClaimsAsync(await _roleManager.FindByIdAsync(roleName)));
            }
            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}
