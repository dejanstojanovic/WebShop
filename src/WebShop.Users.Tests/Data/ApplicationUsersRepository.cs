using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Users.Data;
using Microsoft.AspNetCore.Identity;
using WebShop.Users.Data.Entities;
using WebShop.Users.Data.Repositories;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebShop.Common.Exceptions;


namespace WebShop.Users.Tests.Data
{
    public class ApplicationUsersRepositoryTests
    {

        #region GetUser

        [Fact]
        public async Task GetUser_ReturnsUser()
        {
            var dbSetMock = new Mock<DbSet<ApplicationUser>>();
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(s => s.Set<ApplicationUser>()).Returns(dbSetMock.Object);

            var userManagerMock = GetUserManagerMock<ApplicationUser>();
            userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<String>())).Returns(Task.FromResult(new ApplicationUser()));

            var roleManagerMock = GetRoleManagerMock<IdentityRole>().Object;

            ApplicationUsersRepository applicationUsersRepository = new ApplicationUsersRepository(
                dbContextMock.Object,
                userManagerMock.Object,
                roleManagerMock
                );
            var user = await applicationUsersRepository.GetUser(Guid.NewGuid());
            Assert.NotNull(user);
            Assert.IsAssignableFrom<ApplicationUser>(user);
        }


        [Fact]
        public async Task GetUser_Throws_NotFoundException()
        {
            var dbSetMock = new Mock<DbSet<ApplicationUser>>();
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(s => s.Set<ApplicationUser>()).Returns(dbSetMock.Object);

            var userManagerMock = GetUserManagerMock<ApplicationUser>();
            userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<String>())).Returns(Task.FromResult<ApplicationUser>(null));

            var roleManagerMock = GetRoleManagerMock<IdentityRole>().Object;

            ApplicationUsersRepository applicationUsersRepository = new ApplicationUsersRepository(
                dbContextMock.Object,
                userManagerMock.Object,
                roleManagerMock
                );
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await applicationUsersRepository.GetUser(Guid.NewGuid());
            });

        }

        #endregion

        #region GetUsers

        #endregion

        #region Common methods

        Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser
        {
            return new Mock<UserManager<TIDentityUser>>(
                    new Mock<IUserStore<TIDentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<TIDentityUser>>().Object,
                    new IUserValidator<TIDentityUser>[0],
                    new IPasswordValidator<TIDentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<TIDentityUser>>>().Object);
        }

        Mock<RoleManager<TIdentityRole>> GetRoleManagerMock<TIdentityRole>() where TIdentityRole : IdentityRole
        {
            return new Mock<RoleManager<TIdentityRole>>(
                    new Mock<IRoleStore<TIdentityRole>>().Object,
                    new IRoleValidator<TIdentityRole>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<ILogger<RoleManager<TIdentityRole>>>().Object);
        }
        #endregion

    }
}
