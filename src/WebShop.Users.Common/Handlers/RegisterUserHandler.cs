using WebShop.Users.Common.Commands;
using WebShop.Users.Data.Repositories;
using WebShop.Users.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Users.Data.Entities;
using WebShop.Messaging;
using WebShop.Users.Common.Events;
using AutoMapper;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebShop.Storage;

namespace WebShop.Users.Common.Handlers
{
    public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        private readonly IStorageService _imageStorage;
        private readonly IMessagePublisher<UserRegisteredEvent> _messagePublisher;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(
            IApplicationUsersUnitOfWork applicationUsersUnitOfWork,
            IStorageService imageStorage,
            IMessagePublisher<UserRegisteredEvent> messagePublisher,
            IMapper mapper,
            ILogger<RegisterUserHandler> logger)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
            _imageStorage = imageStorage;
            _messagePublisher = messagePublisher;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task HandleAsync(RegisterUserCommand command)
        {
            Guid id = command.Id != Guid.Empty ? command.Id : Guid.NewGuid();
            await this._applicationUsersUnitOfWork.ApplicationUsers.CreateUser(new ApplicationUser()
            {
                Id = id.ToString(),
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Occupation = command.Occupation,
                Education = command.Education
            }, command.Password);

            if (command.Image != null && command.Image.Any())
            {
                try
                {
                    await _imageStorage.Put(command.Image, $"{id.ToString()}.jpg");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unable to save image for user id {id.ToString()}");
                }
            }

            //Save to DB
            await _applicationUsersUnitOfWork.SaveAsync();

            await _messagePublisher.Publish(_mapper.Map<UserRegisteredEvent>(command));
        }

        public void Dispose() { }

    }
}
