using GetBee.Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GetBee.Users.Data.Repositories
{
    public class ProfileImageRepository : IProfileImageRepository
    {
        private readonly String _folderPath;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly IHostingEnvironment _hostingEnvironment;


        public ProfileImageRepository(IConfiguration configuration, IDistributedCache cache, IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._cache = cache;
            this._configuration = configuration;
            this._folderPath = Path.Combine(Path.GetDirectoryName(hostingEnvironment.ContentRootPath), configuration.GetValue<String>("Profile:Folder"));
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        public async Task<byte[]> GetImage(Guid userId)
        {
            byte[] data = await _cache.GetAsync(userId.ToString());
            if (data == null)
            {
                if (!File.Exists(GetPath(userId)))
                {
                    throw new NotFoundException();
                }
                data = await File.ReadAllBytesAsync(GetPath(userId));
                await _cache.SetAsync(userId.ToString(), data);
            }
            return data;
        }

        public async Task RemoveImage(Guid userId)
        {
            if (!File.Exists(GetPath(userId)))
            {
                throw new NotFoundException();
            }
            File.Delete(GetPath(userId));
            await _cache.RemoveAsync(userId.ToString());
        }

        public async Task SetImage(Guid userId, byte[] image)
        {
            using (var fileStream = new FileStream(GetPath(userId), FileMode.Create))
            {
                await fileStream.WriteAsync(image);
                await _cache.SetAsync(userId.ToString(), image);
            }
        }

        private String GetPath(Guid userId)
        {
            return Path.Combine(_folderPath, $"{userId.ToString()}.jpg");
        }

    }
}
