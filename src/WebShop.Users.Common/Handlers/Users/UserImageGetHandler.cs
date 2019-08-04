using WebShop.Users.Common.Queries;
using WebShop.Users.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;
using WebShop.Storage;

namespace WebShop.Users.Common.Handlers
{
    public class UserImageGetHandler : IQueryHandler<UserImageGetQuery, byte[]>
    {
        private readonly IStorageService _imageStorage;
        public UserImageGetHandler(IStorageService imageStorage)
        {
            _imageStorage = imageStorage;
        }
        public async Task<byte[]> HandleAsync(UserImageGetQuery query)
        {
            var data = await _imageStorage.Get($"{query.UserId.ToString()}.jpg");
            return data;
        }
    }
}
