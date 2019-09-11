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
using WebShop.Common.Extensions;

namespace WebShop.Users.Tests.Data
{
    public class ApplicationUsersRepositoryTests
    {

        #region GetUser

        [Fact]
        public async Task GetUser_ReturnsUser()
        {
            var userId = Guid.Parse("5bd62b43-0668-4821-9b6f-e185271153b4");
            var data = GetMockData();
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(s => s.Set<ApplicationUser>()).Returns(data.GetDbSetMock<ApplicationUser>().Object);

            var userManagerMock = GetUserManagerMock<ApplicationUser>();
            userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<String>())).Returns(
                Task.FromResult(GetMockData().SingleOrDefault(e => e.Id.Equals(userId.ToString())))
                );

            var roleManagerMock = GetRoleManagerMock<IdentityRole>().Object;

            ApplicationUsersRepository applicationUsersRepository = new ApplicationUsersRepository(
                dbContextMock.Object,
                userManagerMock.Object,
                roleManagerMock
                );
            var user = await applicationUsersRepository.GetUser(userId);
            Assert.NotNull(user);
            Assert.IsAssignableFrom<ApplicationUser>(user);
        }


        [Fact]
        public async Task GetUser_Throws_NotFoundException()
        {
            var userId = Guid.Parse("5bd62b43-0668-4821-9b6f-e185271153b5");
            var data = GetMockData();
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(s => s.Set<ApplicationUser>()).Returns(data.GetDbSetMock<ApplicationUser>().Object);

            var userManagerMock = GetUserManagerMock<ApplicationUser>();
            userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<String>())).Returns(
                Task.FromResult(GetMockData().SingleOrDefault(e => e.Id.Equals(userId.ToString())))
                );

            var roleManagerMock = GetRoleManagerMock<IdentityRole>().Object;

            ApplicationUsersRepository applicationUsersRepository = new ApplicationUsersRepository(
                dbContextMock.Object,
                userManagerMock.Object,
                roleManagerMock
                );
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await applicationUsersRepository.GetUser(userId);
            });


        }

        #endregion

        #region GetUsers

        #endregion

        #region Common methods

        //TODO: Duplicate functionality WebShop.Common.Extensions.UnitTesting.GetDbSetMock (to be removed)
        Mock<DbSet<TEntity>> GetDbSetMock<TEntity>(IList<TEntity> entities) where TEntity : IdentityUser
        {
            var queryableEntitites = entities.AsQueryable();
            var dbSet = new Mock<DbSet<TEntity>>();
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryableEntitites.Provider);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryableEntitites.Expression);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryableEntitites.ElementType);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryableEntitites.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<TEntity>())).Callback<TEntity>((s) => entities.Add(s));
            return dbSet;
        }

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

        IList<ApplicationUser> GetMockData()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id =  Guid.Parse("5bd62b43-0668-4821-9b6f-e185271153b4").ToString(),
                    FirstName = "John",
                    LastName = "Wick"
               }
            };
        }

        #endregion

    }
}
