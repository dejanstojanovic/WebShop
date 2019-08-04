using WebShop.Users.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebShop.Users.Data.Repositories
{
    public interface IApplicationUsersRepository
    {
        Task CreateUser(ApplicationUser user, String password);
        Task UpdatePassword(Guid userId, String oldPassword, String newPassword);
        Task<String> GetResetPasswordToken(Guid userId);
        Task ResetPassword(Guid userId, String token, String password);
        Task UpdateProfile(
            Guid userId, 
            String firstName,
            String lastName,
            String occupation,
            String education);
        Task<ApplicationUser> GetUser(Guid userId);
        Task<IEnumerable<ApplicationUser>> GetUsers(
            String firstName,
            String lastName,
            String occupation,
            String education,
            String email,
            int pageindex, int pageSize);
        Task<IEnumerable<String>> GetRoles(Guid userId);
        Task AddRole(Guid userId, String roleName);
        Task AddRole(Guid userId, Guid roleId);
        Task RemoveRole(Guid userId, String roleName);
        Task RemoveRole(Guid userId, Guid roleId);

    }
}
