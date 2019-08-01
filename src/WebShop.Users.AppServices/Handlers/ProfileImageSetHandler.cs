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
    public class ProfileImageSetHandler : ICommandHandler<ProfileImageSetCommand>
    {
        private readonly IStorageService _imageStorage;
        public ProfileImageSetHandler(IStorageService imageStorage)
        {
            _imageStorage = imageStorage;
        }

        public async Task HandleAsync(ProfileImageSetCommand command)
        {
            await _imageStorage.Put(command.Image, $"{command.UserId.ToString()}.jpg");
        }

        public void Dispose() { }
    }
}
