using WebShop.Users.Common.Commands;
using WebShop.Users.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;
using WebShop.Storage;

namespace WebShop.Users.Common.Handlers
{
    public class UserImageSetHandler : ICommandHandler<SetUserImageCommand>
    {
        private readonly IStorageService _imageStorage;
        public UserImageSetHandler(IStorageService imageStorage)
        {
            _imageStorage = imageStorage;
        }

        public async Task HandleAsync(SetUserImageCommand command)
        {
            await _imageStorage.Put(command.Image, $"{command.UserId.ToString()}.jpg");
        }

        public void Dispose() { }
    }
}
