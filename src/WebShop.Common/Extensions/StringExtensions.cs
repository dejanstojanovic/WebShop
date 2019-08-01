using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WebShop.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToSha512(this string input)
        {
            using (var sha = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
