using WebShop.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Storage.FileSystem
{
    public class StorageService : IStorageService
    {
        private readonly String _folderPath;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

           public StorageService(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._configuration = configuration;
            this._folderPath = Path.Combine(Path.GetDirectoryName(hostingEnvironment.ContentRootPath), configuration.GetValue<String>("Profile:Folder"));
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        private String GetPath(String userId)
        {
            return Path.Combine(_folderPath, $"{userId.ToString()}.jpg");
        }

        public async Task<byte[]> Get(string address, string mimeType=null)
        {
                if (!File.Exists(GetPath(address)))
                {
                    throw new NotFoundException();
                }
                return await File.ReadAllBytesAsync(GetPath(address));
        }

        public async Task Put(byte[] data, string address)
        {
            using (var fileStream = new FileStream(GetPath(address), FileMode.Create))
            {
                await fileStream.WriteAsync(data);
            }
        }

        public async Task Remove(string address)
        {
            await Task.CompletedTask;
            if (!File.Exists(GetPath(address)))
            {
                throw new NotFoundException();
            }
            File.Delete(GetPath(address));
        }
    }
}
