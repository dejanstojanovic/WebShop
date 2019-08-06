using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebShop.Web.Mvc.Extensions
{
    public static class ContentConversionExtensions
    {
        public static MultipartFormDataContent ToHttpFileContent(this IFormFile file, String fieldName)
        {
            byte[] data;
            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                data = binaryReader.ReadBytes((int)file.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(bytes, fieldName, file.FileName);
            return multiContent;

        }
    }
}
