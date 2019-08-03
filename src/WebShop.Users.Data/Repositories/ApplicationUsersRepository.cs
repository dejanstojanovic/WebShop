using WebShop.Users.Data.Entities;
using WebShop.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace WebShop.Users.Data.Repositories
{
    public class ApplicationUsersRepository : IApplicationUsersRepository
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUsersRepository(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        public async Task CreateUser(ApplicationUser user, String password)
        {
            if (this._dbContext.Set<ApplicationUser>().Where(u =>
                 u.Id == user.Id ||
                 u.Email == user.Email ||
                 u.UserName == user.UserName).Any())
            {
                throw new DuplicateException();
            }
            user.UserName = user.Email;
            await _userManager.CreateAsync(user, password);
            await _userManager.SetLockoutEnabledAsync(user, false);
        }

        public async Task UpdatePassword(Guid userId, String oldPassword, String newPassword)
        {
            var user = await this.GetUser(userId);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new MismatchException();
            }
        }

        public async Task UpdateProfile(Guid userId,
            String firstName,
            String lastName,
            String occupation,
            String education)
        {
            var user = await this.GetUser(userId);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Occupation = occupation;
            user.Education = education;
        }

        public async Task<ApplicationUser> GetUser(Guid userId)
        {
            var result = await this._userManager.FindByIdAsync(userId.ToString());
            if (result == null)
                throw new NotFoundException();

            return result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers(
            String firstName,
            String lastName,
            String occupation,
            String education,
            String email,
            int pageindex, int pageSize)
        {
            await Task.CompletedTask;
            var result = this._dbContext.Set<ApplicationUser>().Where(
                u =>
                    (firstName == null || firstName == "" || u.FirstName.StartsWith(firstName)) &&
                    (lastName == null || lastName == "" || u.LastName.StartsWith(lastName)) &&
                    (occupation == null || occupation == "" || u.FirstName.StartsWith(occupation)) &&
                    (education == null || education == "" || u.FirstName.StartsWith(education)) &&
                    (email == null || email == "" || u.FirstName == firstName)
                )
                .Skip(pageindex * pageSize)
                .Take(pageSize);

            if (!result.Any())
                throw new NotFoundException();

            return result;

        }

        public async Task<string> GetResetPasswordToken(Guid userId)
        {
            var user = await GetUser(userId);
            if (user != null)
            {
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }
            throw new NotFoundException();
        }

        public async Task ResetPassword(Guid userId, string token, string password)
        {
            var user = await GetUser(userId);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (!result.Succeeded)
                {
                    throw new MismatchException();
                }
            }
            throw new NotFoundException();

            
        }

        public async Task<IEnumerable<String>> GetRoles(Guid userId)
        {
            return await _userManager.GetRolesAsync(await GetUser(userId));
        }
    }
}
