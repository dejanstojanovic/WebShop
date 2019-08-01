using WebShop.Users.AppServices.Commands;
using WebShop.Users.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;
using WebShop.Storage;

namespace WebShop.Users.AppServices.Handlers
{
    public class ProfileImageRemoveHandler : ICommandHandler<ProfileImageRemoveCommand>
    {
        private readonly IStorageService _imageStorage;
        public ProfileImageRemoveHandler(IStorageService imageStorage)
        {
            _imageStorage = imageStorage;
        }

        public async Task HandleAsync(ProfileImageRemoveCommand command)
        {
           await _imageStorage.Remove($"{command.UserId.ToString()}.jpg");
        }

        public void Dispose() { }
    }
}
