using AutoMapper;
using WebShop.Common.Exceptions;
using WebShop.Users.Services.Handlers;
using WebShop.Users.Services.Queries;
using WebShop.Users.Dtos.ApplicationUser;
using WebShop.Users.Data.Entities;
using WebShop.Users.Data.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using WebShop.Messaging;
using WebShop.Users.Data;

namespace WebShop.Users.Tests.Handlers
{
    public class QueryHandlerTests
    {
        [Fact]
        public async Task GetProfile_ReturnsUserProfile_WhenUserFound()
        {

            //Arrange
            Mock<IApplicationUserRepository> applicationRepository = new Mock<IApplicationUserRepository>();
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();

            applicationRepository.Setup(s => s.GetUser(It.IsAny<Guid>())).ReturnsAsync(new ApplicationUser());
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);
            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<ProfileView>(It.IsAny<Object>())).Returns(new ProfileView());

            var handler = new ProfileGetHandler(
                unitOfWork.Object,
                mapper.Object
                );

            //Act
            var result = await handler.HandleAsync(new ProfileGetQuery());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ProfileView>(result);
        }

        [Fact]
        public async Task GetProfile_ThorowsNotFoundException_WhenUserNotFound()
        {
            //Arrange
            Mock<IApplicationUserRepository> applicationRepository = new Mock<IApplicationUserRepository>();
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();

            applicationRepository.Setup(s => s.GetUser(It.IsAny<Guid>())).Throws<NotFoundException>();
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);

            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<ProfileView>(It.IsAny<Object>())).Returns(new ProfileView());

            var handler = new ProfileGetHandler(
                unitOfWork.Object,
                mapper.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.HandleAsync(new ProfileGetQuery());
            });
        }

        [Fact]
        public async Task GetProfiles_ReturnsUserProfiles_WhenUserFound()
        {
            //Arrange
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();
            Mock<IApplicationUserRepository> applicationRepository = new Mock<IApplicationUserRepository>();
            applicationRepository.Setup(s => s.GetUsers(
                It.IsAny<String>(),
                 It.IsAny<String>(),
                 It.IsAny<String>(),
                 It.IsAny<String>(),
                 It.IsAny<String>(),
                 It.IsAny<int>(),
                 It.IsAny<int>()
                )).ReturnsAsync(new List<ApplicationUser>());
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);

            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<IEnumerable<ProfileView>>(It.IsAny<Object>())).Returns(new List<ProfileView>());

            var handler = new ProfileBrowseHandler(
                unitOfWork.Object,
                mapper.Object
                );

            //Act
            var result = await handler.HandleAsync(new ProfileBrowseQuery(new ProfileBrowse()));

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ProfileView>>(result);
        }

        [Fact]
        public async Task GetProfiles_ThorowsNotFoundException_WhenUserNotFound()
        {
            //Arrange
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();
            Mock<IApplicationUserRepository> applicationRepository = new Mock<IApplicationUserRepository>();
            applicationRepository.Setup(s => s.GetUsers(
                It.IsAny<String>(),
                 It.IsAny<String>(),
                 It.IsAny<String>(),
                 It.IsAny<String>(),
                 It.IsAny<String>(),
                 It.IsAny<int>(),
                 It.IsAny<int>()
                )).Throws<NotFoundException>();
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);
            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<IEnumerable<ProfileView>>(It.IsAny<Object>())).Returns(new List<ProfileView>());

            var handler = new ProfileBrowseHandler(
                unitOfWork.Object,
                mapper.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.HandleAsync(new ProfileBrowseQuery(new ProfileBrowse()));
            });

        }

    }




}
