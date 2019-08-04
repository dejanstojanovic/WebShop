using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebShop.Users.Data.Entities;
using WebShop.Users.Data.Values;
using System.Linq;

namespace WebShop.Users.Data.Repositories
{
    public class ApplicationRolesRepository:IApplicationRolesRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationRolesRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddClaim(string roleName, RoleClaim claim)
        {
            await _roleManager.AddClaimAsync(await GetRole(roleName), new Claim(claim.ClaimType, claim.ClaimValue));
        }

        public async Task AddRole(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
        }

        public async Task<IEnumerable<RoleClaim>> GetClaims(string roleName)
        {
            return (await _roleManager.GetClaimsAsync(await GetRole(roleName))).Select(c=>new RoleClaim() { ClaimType = c.ValueType, ClaimValue = c.Value });
        }

        public async Task<IdentityRole> GetRole(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<IdentityRole> GetRole(Guid roleId)
        {
            return await _roleManager.FindByIdAsync(roleId.ToString());
        }

        public async Task RemoveClaim(string roleName, RoleClaim claim)
        {
            await _roleManager.RemoveClaimAsync(await GetRole(roleName), new Claim(claim.ClaimType, claim.ClaimValue));
        }

        public async Task RemoveRole(string name)
        {
            await _roleManager.DeleteAsync(await GetRole(name));
        }

        public async Task RemoveRole(Guid roleId)
        {
            await _roleManager.DeleteAsync(await GetRole(roleId));
        }
    }
}
