using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Users.Data.Entities;
using WebShop.Users.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebShop.Common.Database;

namespace WebShop.Users.Data
{
    public class ApplicationUsersUnitOfWork : IApplicationUsersUnitOfWork
    {
        bool _disposing = false;
        private readonly IApplicationUsersRepository _applicationUsers;
        private readonly IApplicationRolesRepository _applicationRoles;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserStore<ApplicationUser, IdentityRole, ApplicationDbContext, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>> _userStore;
        private readonly RoleStore<IdentityRole, ApplicationDbContext, string, IdentityUserRole<string>, IdentityRoleClaim<string>> _roleStore;

        public ApplicationUsersUnitOfWork(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUserStore<ApplicationUser> userStore,
        IRoleStore<IdentityRole> roleStore)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleStore = roleStore as RoleStore<IdentityRole, ApplicationDbContext, string, IdentityUserRole<string>, IdentityRoleClaim<string>>;
            _userStore = userStore as UserStore<ApplicationUser, IdentityRole, ApplicationDbContext, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>;
            _userStore.AutoSaveChanges = false;
            _roleStore.AutoSaveChanges = false;

            _applicationUsers = new ApplicationUsersRepository(_userStore.Context, _userManager, _roleManager);
            _applicationRoles = new ApplicationRolesRepository(_userManager, _roleManager);
        }

        public IDatabaseTransaction BeginTransaction => new EntityDatabaseTransaction(_userStore.Context);

        public IApplicationUsersRepository ApplicationUsers => _applicationUsers;

        public async Task SaveAsync()
        {
            await _userStore.Context.SaveChangesAsync();
            //await _roleStore.Context.SaveChangesAsync();
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
