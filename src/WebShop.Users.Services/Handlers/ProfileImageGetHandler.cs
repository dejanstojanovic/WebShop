using WebShop.Users.Services.Queries;
using WebShop.Users.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;
using WebShop.Storage;

namespace WebShop.Users.Services.Handlers
{
    public class ProfileImageGetHandler : IQueryHandler<ProfileImageGetQuery, byte[]>
    {
        private readonly IStorageService _imageStorage;
        public ProfileImageGetHandler(IStorageService imageStorage)
        {
            _imageStorage = imageStorage;
        }
        public async Task<byte[]> HandleAsync(ProfileImageGetQuery query)
        {
            var data = await _imageStorage.Get($"{query.UserId.ToString()}.jpg");
            return data;
        }
    }
}
