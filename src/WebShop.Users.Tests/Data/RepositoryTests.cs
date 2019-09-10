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

namespace WebShop.Users.Tests.Data
{
    public class RepositoryTests
    {



        [Fact]
        public async Task GetUser()
        {
            try
            {
                var userId = Guid.Parse("5bd62b43-0668-4821-9b6f-e185271153b4");

                var dbContextMock = GetDbContextMock<ApplicationUser>(GetMockData()).Object;
                var userManagerMock = GetUserManagerMock();

                userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<String>())).Returns(
                    Task.FromResult(GetMockData().SingleOrDefault(e => e.Id.Equals(userId.ToString())))
                    );

                var roleManagerMock = GetRoleManagerMock().Object;

                ApplicationUsersRepository applicationUsersRepository = new ApplicationUsersRepository(
                    dbContextMock,
                    userManagerMock.Object,
                    roleManagerMock
                    );
                var user = applicationUsersRepository.GetUser(userId);
                Assert.NotNull(user);
                Assert.IsAssignableFrom<ApplicationUser>(user);

            }
            catch(Exception ex)
            {
                //Breakpoint here
            }
            await Task.CompletedTask;
        }


        Mock<ApplicationDbContext> GetDbContextMock<T>(IList<T> data) where T : IdentityUser
        {
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(s => s.Set<T>()).Returns(GetDbSetMock<T>(data).Object);
            return dbContextMock;
        }


        Mock<DbSet<T>> GetDbSetMock<T>(IList<T> data) where T : IdentityUser
        {
            var queryable = data.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => data.Add(s));

            return dbSet;
        }

        Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            return new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
        }

        Mock<RoleManager<IdentityRole>> GetRoleManagerMock()
        {
            return new Mock<RoleManager<IdentityRole>>(
                    new Mock<IRoleStore<IdentityRole>>().Object,
                    new IRoleValidator<IdentityRole>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
        }

        IList<ApplicationUser> GetMockData()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id =  Guid.Parse("5bd62b43-0668-4821-9b6f-e185271153b4").ToString(),
                    FirstName = "John",
                    LastName = "Doe"
               }
            };
        }

    }
}
