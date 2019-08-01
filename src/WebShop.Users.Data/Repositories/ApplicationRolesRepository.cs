using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Users.Data.Entities;

namespace WebShop.Users.Data.Repositories
{
    public class ApplicationRolesRepository:IApplicationRoleRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationRolesRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddRole(IdentityRole role)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityRole> GetRole(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityRole> GetRole(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRole(string name)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRole(Guid roleId)
        {
            throw new NotImplementedException();
        }
    }
}
