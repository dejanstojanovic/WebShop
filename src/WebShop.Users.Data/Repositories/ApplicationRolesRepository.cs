using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebShop.Users.Data.Entities;

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

        public async Task AddClaim(string roleName, Claim claim)
        {
            throw new NotImplementedException();
        }

        public async Task AddRole(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
        }

        public async Task<IEnumerable<Claim>> GetClaims(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityRole> GetRole(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<IdentityRole> GetRole(Guid roleId)
        {
            return await _roleManager.FindByIdAsync(roleId.ToString());
        }

        public Task RemoveClaim(string roleName, string claimType, string claimValue)
        {
            throw new NotImplementedException();
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
