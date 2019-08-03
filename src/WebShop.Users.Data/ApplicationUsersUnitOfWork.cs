using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Users.Data.Entities;
using WebShop.Users.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebShop.Users.Data
{
    public class ApplicationUsersUnitOfWork : IApplicationUsersUnitOfWork
    {
        bool _disposing = false;
        private readonly IApplicationUsersRepository _applicationUsers;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserStore<ApplicationUser, IdentityRole, ApplicationDbContext, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>> _userStore;

            public ApplicationUsersUnitOfWork(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore)
        {
            _userManager = userManager;
            _userStore = userStore as UserStore<ApplicationUser, IdentityRole, ApplicationDbContext, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>;
            _userStore.AutoSaveChanges = false;
            _applicationUsers = new ApplicationUsersRepository(_userStore.Context, userManager);
        }

        public IDatabaseTransaction BeginTransaction => new EntityDatabaseTransaction(_userStore.Context);

        public IApplicationUsersRepository ApplicationUsers => _applicationUsers;

        public async Task<int> SaveAsync()
        {
            return await _userStore.Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (!_disposing)
            {
                _userStore.Dispose();
                _disposing = true;
            }
        }
    }
}
