using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Storage.FileSystem.Extensions
{
    public static class FileStorageExtensions
    {
        public static IServiceCollection AddFileSystemStorage(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));
            
            services.AddScoped<IStorageService, StorageService>();
            

            return services;
        }
    }
}
