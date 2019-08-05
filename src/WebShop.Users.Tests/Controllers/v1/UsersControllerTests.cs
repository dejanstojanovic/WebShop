using WebShop.Common.Exceptions;
using WebShop.Users.Api.Controllers.v1;
using WebShop.Users.Common;
using WebShop.Users.Common.Commands;
using WebShop.Users.Common.Queries;
using WebShop.Users.Common.Dtos.Users;
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
            var result = await controller.RegisterUser(new RegisterUserCommand(
                id:Guid.NewGuid(),
                email:string.Empty,
                firstName:String.Empty,
                lastName:String.Empty,
                occupation:String.Empty,
                education:String.Empty,
                password:String.Empty,
                dateOfBirth:DateTime.Now,
                image:null
                ) { });

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
                await controller.RegisterUser(new RegisterUserCommand(
                id: Guid.NewGuid(),
                email: string.Empty,
                firstName: String.Empty,
                lastName: String.Empty,
                occupation: String.Empty,
                education: String.Empty,
                password: String.Empty,
                dateOfBirth: DateTime.Now,
                image: null
                )
                { });
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
            queryDispatcher.Setup(s => s.HandleAsync<UserGetQuery, UserInfoDetailsViewDto>(It.IsAny<UserGetQuery>())).ReturnsAsync(new UserInfoDetailsViewDto());
            var controller = new UsersController(
                queryDispatcher: queryDispatcher.Object
                );

            //Act
            var result = await controller.FindUserById(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenUserNotFound()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<UserGetQuery, UserInfoDetailsViewDto>(It.IsAny<UserGetQuery>())).Throws<NotFoundException>();
            var controller = new UsersController(
                queryDispatcher: queryDispatcher.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.FindUserById(Guid.NewGuid());
            });

            //NOTE: NotFoundException response handled byt the pipeline
        }

        [Fact]
        public async Task Get_ReturnsOkCollectionOfProfileView_WhenAnyUserFound()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<UserFilterQuery, IEnumerable<UserInfoDetailsViewDto>>(It.IsAny<UserFilterQuery>())).ReturnsAsync(new List<UserInfoDetailsViewDto>() { new UserInfoDetailsViewDto() });
            var controller = new UsersController(
                queryDispatcher: queryDispatcher.Object
                );

            //Execute
            var result = await controller.FindUsers(new UserFilterQuery());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenUsersNotFound()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<UserFilterQuery, IEnumerable<UserInfoDetailsViewDto>>(It.IsAny<UserFilterQuery>())).Throws<NotFoundException>();
            var controller = new UsersController(
                queryDispatcher: queryDispatcher.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.FindUsers(new UserFilterQuery());
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
            commandDispatcher.Setup(s => s.HandleAsync<UpdateUserInfoCommand>(It.IsAny<UpdateUserInfoCommand>())).Returns(Task.CompletedTask);
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );
            SetAuthenticationContext(controller);
            //Act
            var result = await controller.UpdateUserInfo(Guid.NewGuid(),new UpdateUserInfoCommand(
                id:Guid.Empty,
                firstName:String.Empty,
                lastName:String.Empty,
                education:String.Empty,
                occupation:String.Empty,
                dateOfBirth:DateTime.Now
                ));

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsNotFound_WhenProfileNotFound()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            commandDispatcher.Setup(s => s.HandleAsync<UpdateUserInfoCommand>(It.IsAny<UpdateUserInfoCommand>())).Throws<NotFoundException>();
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );
            SetAuthenticationContext(controller);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.UpdateUserInfo(Guid.NewGuid(),new UpdateUserInfoCommand(
                id: Guid.Empty,
                firstName: String.Empty,
                lastName: String.Empty,
                education: String.Empty,
                occupation: String.Empty,
                dateOfBirth: DateTime.Now
                ));
            });

            //NOTE: NotFoundException response handled byt the pipeline
        }


        [Fact]
        public async Task UpdatePassword_ReturnsOk_WhenProfileUpdated()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            commandDispatcher.Setup(s => s.HandleAsync<UpdateUserPasswordCommand>(It.IsAny<UpdateUserPasswordCommand>())).Returns(Task.CompletedTask);
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );

            SetAuthenticationContext(controller);

            //Act
            var result = await controller.UpdateUserPassword(Guid.NewGuid(),new UpdateUserPasswordCommand(
                userId:Guid.Empty,
                oldPassword:String.Empty,
                newPassword:String.Empty
                ));

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsNotFound_WhenProfileNotFound()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            commandDispatcher.Setup(s => s.HandleAsync<UpdateUserPasswordCommand>(It.IsAny<UpdateUserPasswordCommand>())).Throws<NotFoundException>();
            var controller = new UsersController(
                commandDispatcher: commandDispatcher.Object
                );

            SetAuthenticationContext(controller);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.UpdateUserPassword(Guid.Empty,new UpdateUserPasswordCommand(
                userId: Guid.Empty,
                oldPassword: String.Empty,
                newPassword: String.Empty
                ));
            });

            //NOTE: NotFoundException response handled byt the pipeline
        }


        #endregion


    }
}
