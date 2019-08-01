using WebShop.Users.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Users.Data.Repositories
{
    public interface IApplicationUserRepository
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
    }
}
