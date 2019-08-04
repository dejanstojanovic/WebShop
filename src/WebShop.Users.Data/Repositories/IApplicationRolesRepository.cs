using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebShop.Users.Data.Values;

namespace WebShop.Users.Data.Repositories
{
    public interface IApplicationRolesRepository
    {
        Task AddRole(IdentityRole role);
        Task<IdentityRole> GetRole(String name);
        Task<IdentityRole> GetRole(Guid roleId);
        Task RemoveRole(String name);
        Task RemoveRole(Guid roleId);
        Task AddClaim(String roleName, RoleClaim claim);
        Task RemoveClaim(String roleName, RoleClaim claim);
        Task<IEnumerable<RoleClaim>> GetClaims(string roleName);
    }
}
