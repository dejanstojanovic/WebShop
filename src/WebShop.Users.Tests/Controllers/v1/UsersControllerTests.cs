using WebShop.Common.Exceptions;
using WebShop.Users.Api.Controllers.v1;
using WebShop.Users.Services;
using WebShop.Users.Services.Commands;
using WebShop.Users.Services.Queries;
using WebShop.Users.Dtos.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using WebShop.Messaging;

namespace WebShop.Users.Tests.Controllers.v1
{
    public class UsersControllerTests:BaseControllerTests
    {
        readonly Mock<IConfiguration> configuration;
        readonly Mock<ILogger<UsersController>> logger;

        public UsersControllerTests()
        {
            this.configuration = new Mock<IConfiguration>();
            this.logger = new Mock<ILogger<UsersController>>();
        }

        #region POST
        [Fact]
        public async Task CreateUser_ReturnsCreatedAt_WhenUserCreated()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            commandDispatcher.Setup(s => s.HandleAsync<RegisterUserCommand>(It.IsAny<RegisterUserCommand>())).Returns(Task.CompletedTask);
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );

            //Act
            var result = await controller.Register(new Register() { });

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task CreateUser_ReturnsConflict_WhenUserExists()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            commandDispatcher.Setup(s => s.HandleAsync<RegisterUserCommand>(It.IsAny<RegisterUserCommand>())).Throws<DuplicateException>();
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<DuplicateException>(async () =>
            {
                await controller.Register(new Register() { });
            });

            //NOTE: DuplicateException response handled byt the pipeline
        }

        #endregion

        #region GET
        [Fact]
        public async Task GetUser_ReturnsOkProfileView_WhenUserFound()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<ProfileGetQuery, ProfileView>(It.IsAny<ProfileGetQuery>())).ReturnsAsync(new ProfileView());
            var controller = new UsersController(
                queryDispatcher: queryDispatcher.Object
                );

            //Act
            var result = await controller.GetById(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenUserNotFound()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<ProfileGetQuery, ProfileView>(It.IsAny<ProfileGetQuery>())).Throws<NotFoundException>();
            var controller = new UsersController(
                queryDispatcher: queryDispatcher.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.GetById(Guid.NewGuid());
            });

            //NOTE: NotFoundException response handled byt the pipeline
        }

        [Fact]
        public async Task Get_ReturnsOkCollectionOfProfileView_WhenAnyUserFound()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<ProfileBrowseQuery, IEnumerable<ProfileView>>(It.IsAny<ProfileBrowseQuery>())).ReturnsAsync(new List<ProfileView>() { new ProfileView() });
            var controller = new UsersController(
                queryDispatcher: queryDispatcher.Object
                );

            //Execute
            var result = await controller.GetUsers(new ProfileBrowse());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenUsersNotFound()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<ProfileBrowseQuery, IEnumerable<ProfileView>>(It.IsAny<ProfileBrowseQuery>())).Throws<NotFoundException>();
            var controller = new UsersController(
                queryDispatcher: queryDispatcher.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.GetUsers(new ProfileBrowse());
            });

            //NOTE: NotFoundException response handled byt the pipeline
        }

        #endregion

        #region PUT
        [Fact]
        public async Task UpdateProfile_ReturnsOk_WhenProfileUpdated()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            commandDispatcher.Setup(s => s.HandleAsync<UpdateProfileCommand>(It.IsAny<UpdateProfileCommand>())).Returns(Task.CompletedTask);
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );
            SetAuthenticationContext(controller);
            //Act
            var result = await controller.UpdateProfile(new ProfileUpdate());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsNotFound_WhenProfileNotFound()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            commandDispatcher.Setup(s => s.HandleAsync<UpdateProfileCommand>(It.IsAny<UpdateProfileCommand>())).Throws<NotFoundException>();
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );
            SetAuthenticationContext(controller);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.UpdateProfile(new ProfileUpdate());
            });

            //NOTE: NotFoundException response handled byt the pipeline
        }


        [Fact]
        public async Task UpdatePassword_ReturnsOk_WhenProfileUpdated()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            commandDispatcher.Setup(s => s.HandleAsync<UpdatePasswordCommand>(It.IsAny<UpdatePasswordCommand>())).Returns(Task.CompletedTask);
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );

            SetAuthenticationContext(controller);

            //Act
            var result = await controller.UpdatePassword(new PasswordUpdate());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsNotFound_WhenProfileNotFound()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            commandDispatcher.Setup(s => s.HandleAsync<UpdatePasswordCommand>(It.IsAny<UpdatePasswordCommand>())).Throws<NotFoundException>();
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );

            SetAuthenticationContext(controller);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.UpdatePassword(new PasswordUpdate());
            });

            //NOTE: NotFoundException response handled byt the pipeline
        }


        #endregion


    }
}
