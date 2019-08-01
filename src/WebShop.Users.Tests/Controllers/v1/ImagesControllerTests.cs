using WebShop.Common.Exceptions;
using WebShop.Users.Api.Controllers.v1;
using WebShop.Users.Services;
using WebShop.Users.Services.Commands;
using WebShop.Users.Services.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using WebShop.Messaging;

namespace WebShop.Users.Tests.Controllers.v1
{
    public class ImagesControllerTests : BaseControllerTests
    {
        readonly Mock<IConfiguration> configuration;
        readonly Mock<IDistributedCache> cache;

        public ImagesControllerTests()
        {
            this.configuration = new Mock<IConfiguration>();
            this.cache = new Mock<IDistributedCache>();
        }

        #region POST
        [Fact]
        public async Task SetProfileImage_ReturnsCreatedAt_WhenImageStored()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            Mock<IFormFile> file = new Mock<IFormFile>();
            commandDispatcher.Setup(s => s.HandleAsync<ProfileImageSetCommand>(It.IsAny<ProfileImageSetCommand>())).Returns(Task.CompletedTask);
            var controller = new ImagesController(
                commandDispatcher: commandDispatcher.Object
                );
            SetAuthenticationContext(controller);
            //Act

            var result = await controller.PostImage(file.Object);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<CreatedAtRouteResult>(result);
        }
        #endregion

        #region GET
        [Fact]
        public async Task GetProfileImage_ReturnsFileResult_WhenImageExists()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<ProfileImageGetQuery,byte[]>(It.IsAny<ProfileImageGetQuery>())).ReturnsAsync(new byte[] { });
            var controller = new ImagesController(
                queryDispatcher: queryDispatcher.Object
                );
            //Act

            var result = await controller.GetImage(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FileContentResult>(result);
        }

        [Fact]
        public async Task GetProfileImage_ThrowsNotFoundException_WhenImageDoesNotExist()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<ProfileImageGetQuery, byte[]>(It.IsAny<ProfileImageGetQuery>())).Throws<NotFoundException>();
            var controller = new ImagesController(
                queryDispatcher: queryDispatcher.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.GetImage(Guid.NewGuid());
            });

        }


        [Fact]
        public async Task GetProfileImageBase64_ReturnsBase64String_WhenImageExists()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<ProfileImageGetQuery, byte[]>(It.IsAny<ProfileImageGetQuery>())).ReturnsAsync(new byte[] { });
            var controller = new ImagesController(
                queryDispatcher: queryDispatcher.Object
                );
            //Act

            var result = await controller.GetBase64(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetProfileImageBase64_ThrowsNotFoundException_WhenImageDoesNotExist()
        {
            //Arrange
            Mock<IQueryDispatcher> queryDispatcher = new Mock<IQueryDispatcher>();
            queryDispatcher.Setup(s => s.HandleAsync<ProfileImageGetQuery, byte[]>(It.IsAny<ProfileImageGetQuery>())).Throws<NotFoundException>();
            var controller = new ImagesController(
                queryDispatcher: queryDispatcher.Object
                );

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.GetBase64(Guid.NewGuid());
            });

        }


        #endregion

        #region DELETE

        [Fact]
        public async Task DeleteProfileImage_ReturnsNoContent_WhenImageRemoved()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            Mock<IFormFile> file = new Mock<IFormFile>();
            commandDispatcher.Setup(s => s.HandleAsync<ProfileImageRemoveCommand>(It.IsAny<ProfileImageRemoveCommand>())).Returns(Task.CompletedTask);
            var controller = new ImagesController(
                commandDispatcher: commandDispatcher.Object
                );
            SetAuthenticationContext(controller);
            //Act

            var result = await controller.Delete();

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProfileImage_ThrowsNotFoundException_WhenImageDoesNotExist()
        {
            //Arrange
            Mock<ICommandDispatcher> commandDispatcher = new Mock<ICommandDispatcher>();
            Mock<IFormFile> file = new Mock<IFormFile>();
            commandDispatcher.Setup(s => s.HandleAsync<ProfileImageRemoveCommand>(It.IsAny<ProfileImageRemoveCommand>())).Throws<NotFoundException>();
            var controller = new ImagesController(
                commandDispatcher: commandDispatcher.Object
                );
            SetAuthenticationContext(controller);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.Delete();
            });


        }


        #endregion


    }
}
