using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Storage
{
   public interface IStorageService
    {
        Task<byte[]> Get(String address, String mimeType=null);
        Task Put(byte[] data, String address);
        Task Remove(String address);
    }
}
