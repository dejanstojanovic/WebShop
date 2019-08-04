using WebShop.Common.Exceptions;
using WebShop.Users.Common.Commands;
using WebShop.Users.Common.Handlers;
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
using AutoMapper;
using WebShop.Users.Common.Events;
using WebShop.Users.Data;
using WebShop.Storage;
using Microsoft.Extensions.Logging;

namespace WebShop.Users.Tests.Handlers
{
    public class CommandHandlerTests
    {
        [Fact]
        public async Task RegisterUser_ReturnsTask_WhenUserRegistered()
        {

            //Arrange
            Mock<IApplicationUsersRepository> applicationRepository = new Mock<IApplicationUsersRepository>();
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();
            Mock<IStorageService> storageService = new Mock<IStorageService>();
            Mock<ILogger<UserRegisterHandler>> logger = new Mock<ILogger<UserRegisterHandler>>();

            applicationRepository.Setup(r => r.CreateUser(It.IsAny<ApplicationUser>(), It.IsAny<String>())).Returns(Task.CompletedTask);
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);
            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<UserRegisteredEvent>(It.IsAny<Object>())).Returns(new UserRegisteredEvent(Guid.NewGuid(), String.Empty, String.Empty, String.Empty));
            Mock<IMessagePublisher<UserRegisteredEvent>> messagePublisher = new Mock<IMessagePublisher<UserRegisteredEvent>>();
            messagePublisher.Setup(p => p.Publish(It.IsAny<UserRegisteredEvent>())).Returns(Task.CompletedTask);

            var handler = new UserRegisterHandler(
                unitOfWork.Object,
                storageService.Object,
                messagePublisher.Object,
                mapper.Object,
                logger.Object);

            //Act
             await handler.HandleAsync(new RegisterUserCommand(new UserRegisterDto()));

            //Assert
            
        }

        [Fact]
        public async Task RegisterUser_ThrowsDuplicateException_WhenUserAlredyExists()
        {
            //Arrange
            Mock<IApplicationUsersRepository> applicationRepository = new Mock<IApplicationUsersRepository>();
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();
            Mock<IStorageService> storageService = new Mock<IStorageService>();
            Mock<ILogger<UserRegisterHandler>> logger = new Mock<ILogger<UserRegisterHandler>>();

            applicationRepository.Setup(r => r.CreateUser(It.IsAny<ApplicationUser>(), It.IsAny<String>())).Throws<DuplicateException>();
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);

            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<UserRegisteredEvent>(It.IsAny<Object>())).Returns(new UserRegisteredEvent(Guid.NewGuid(),String.Empty,String.Empty,String.Empty));
            Mock<IMessagePublisher<UserRegisteredEvent>> messagePublisher = new Mock<IMessagePublisher<UserRegisteredEvent>>();
            messagePublisher.Setup(p => p.Publish(It.IsAny<UserRegisteredEvent>())).Returns(Task.CompletedTask);


            var handler = new UserRegisterHandler(
                unitOfWork.Object,
                storageService.Object,
                messagePublisher.Object,
                mapper.Object,
                logger.Object);

            //Act/Assert
            await Assert.ThrowsAsync<DuplicateException>(async () =>
            {
                await handler.HandleAsync(new RegisterUserCommand(new UserRegisterDto()));
            });

        }

        [Fact]
        public async Task UpdateUserProfile_ReturnsTask_WhenProfileUpdated()
        {
            //Arrange
            Mock<IApplicationUsersRepository> applicationRepository = new Mock<IApplicationUsersRepository>();
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();
            applicationRepository.Setup(r => r.UpdateProfile(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(),It.IsAny<String>())).Returns(Task.CompletedTask);
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);

            var handler = new UserInfoUpdateHandler(unitOfWork.Object);

            //Act
            await handler.HandleAsync(new UpdateUserInfoCommand(Guid.NewGuid(),new UserInfoUpdateDto()));

            //Assert
        }

        [Fact]
        public async Task UpdateUserProfile_ThrowsNotFoundExceptionk_WhenUserNotFound()
        {
            //Arrange
            Mock<IApplicationUsersRepository> applicationRepository = new Mock<IApplicationUsersRepository>();
            Mock<IApplicationUsersUnitOfWork> unitOfWork = new Mock<IApplicationUsersUnitOfWork>();
            applicationRepository.Setup(r => r.UpdateProfile(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Throws<NotFoundException>();
            unitOfWork.Setup(u => u.ApplicationUsers).Returns(applicationRepository.Object);
            var handler = new UserInfoUpdateHandler(unitOfWork.Object);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.HandleAsync(new UpdateUserInfoCommand(Guid.NewGuid(), new UserInfoUpdateDto()));
            });
        }



    }
}
