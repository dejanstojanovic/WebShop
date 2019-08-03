using WebShop.Users.Data.Entities;
using WebShop.Users.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebShop.Users.Data.Extensions
{
    public static class DataExtensions
    {
        public static void AddSqlServer(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();

            var migrationsAssembly = typeof(IApplicationUsersRepository).Assembly;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("WebShop.Users"),
                    sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly.GetName().Name))
                );

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        }

    }
}
