using WebShop.Common.Exceptions;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace WebShop.Storage.FileSystem
{
    public class StorageService : IStorageService
    {
        private readonly String _folderPath;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly FileStorageOptions _fileStorageOptions;

        public StorageService(IOptions<FileStorageOptions> options, IHostingEnvironment hostingEnvironment)
        {
            _fileStorageOptions = options.Value;
            if (String.IsNullOrWhiteSpace(_fileStorageOptions.Folder)) throw new ArgumentNullException("Folder", "Folder path not configured");
            this._hostingEnvironment = hostingEnvironment;
           
            this._folderPath = Path.Combine(Path.GetDirectoryName(hostingEnvironment.ContentRootPath), _fileStorageOptions.Folder);
            if (!Directory.Exists(_folderPath) && _fileStorageOptions.CreateMissing)
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
