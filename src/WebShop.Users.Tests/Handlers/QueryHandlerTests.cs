using AutoMapper;
using WebShop.Common.Exceptions;
using WebShop.Users.Common.Handlers;
using WebShop.Users.Common.Queries;
using WebShop.Users.Common.Dtos.Users;
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
            Mock<IApplicationUsersRepository> applicationRepository = new Mock<IApplicationUsersRepository>();
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();

            applicationRepository.Setup(s => s.GetUser(It.IsAny<Guid>())).ReturnsAsync(new ApplicationUser());
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);
            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<UserInfoDetailsViewDto>(It.IsAny<Object>())).Returns(new UserInfoDetailsViewDto());

            var handler = new UserGetHandler(
                unitOfWork.Object,
                mapper.Object
                );

            //Act
            var result = await handler.HandleAsync(new UserGetQuery());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<UserInfoDetailsViewDto>(result);
        }

        [Fact]
        public async Task GetProfile_ThorowsNotFoundException_WhenUserNotFound()
        {
            //Arrange
            Mock<IApplicationUsersRepository> applicationRepository = new Mock<IApplicationUsersRepository>();
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();

            applicationRepository.Setup(s => s.GetUser(It.IsAny<Guid>())).Throws<NotFoundException>();
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);

            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<UserInfoDetailsViewDto>(It.IsAny<Object>())).Returns(new UserInfoDetailsViewDto());

            var handler = new UserGetHandler(
                unitOfWork.Object,
                mapper.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.HandleAsync(new UserGetQuery());
            });
        }

        [Fact]
        public async Task GetProfiles_ReturnsUserProfiles_WhenUserFound()
        {
            //Arrange
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();
            Mock<IApplicationUsersRepository> applicationRepository = new Mock<IApplicationUsersRepository>();
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
            mapper.Setup(m => m.Map<IEnumerable<UserInfoDetailsViewDto>>(It.IsAny<Object>())).Returns(new List<UserInfoDetailsViewDto>());

            var handler = new UserBrowseHandler(
                unitOfWork.Object,
                mapper.Object
                );

            //Act
            var result = await handler.HandleAsync(new UserFilterQuery(new UserInfoViewDto()));

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<UserInfoDetailsViewDto>>(result);
        }

        [Fact]
        public async Task GetProfiles_ThorowsNotFoundException_WhenUserNotFound()
        {
            //Arrange
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();
            Mock<IApplicationUsersRepository> applicationRepository = new Mock<IApplicationUsersRepository>();
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
            mapper.Setup(m => m.Map<IEnumerable<UserInfoDetailsViewDto>>(It.IsAny<Object>())).Returns(new List<UserInfoDetailsViewDto>());

            var handler = new UserBrowseHandler(
                unitOfWork.Object,
                mapper.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.HandleAsync(new UserFilterQuery(new UserInfoViewDto()));
            });

        }

    }




}
