using WebShop.Users.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebShop.Auth.Api.Services
{
    public interface IUserValidator
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<ApplicationUser> FindByUsernameAsync(string username);
        //Task<ApplicationUser> FindUserFromExternalProviderAsync(string provider, string userId);
        Task<(ApplicationUser user, string provider, string providerUserId, IEnumerable<Claim> claims)> FindUserFromExternalProviderAsync(AuthenticateResult authenticationResult);
        Task<ApplicationUser> AutoProvisionUserAsync(string provider, string userId, IEnumerable<Claim> claims);
    }
}
