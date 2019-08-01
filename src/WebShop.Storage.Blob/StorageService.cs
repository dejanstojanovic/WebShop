using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetBee.Storage.Blob
{
    public class StorageService : IStorageService
    {
        public Task<byte[]> Get(string address, string mimeType=null)
        {
            throw new NotImplementedException();
        }

        public Task Put(byte[] data, string address)
        {
            throw new NotImplementedException();
        }

        public Task Remove(string address)
        {
            throw new NotImplementedException();
        }
    }
}
