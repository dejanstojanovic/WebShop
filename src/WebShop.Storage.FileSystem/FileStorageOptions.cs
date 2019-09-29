using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Storage.FileSystem
{
    public class FileStorageOptions
    {
        public String Folder { get; set; }
        public Boolean CreateMissing { get; set; }
    }
}
