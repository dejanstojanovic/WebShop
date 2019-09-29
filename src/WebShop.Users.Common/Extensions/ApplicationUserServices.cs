using WebShop.Messaging.Extensions;
using WebShop.Messaging.ServiceBus.Extensions;
using WebShop.Storage;
using WebShop.Storage.FileSystem;
using WebShop.Users.Common.Events;
using WebShop.Users.Data;
using WebShop.Users.Data.Entities;
using WebShop.Users.Data.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using WebShop.Storage.FileSystem.Extensions;

namespace WebShop.Users.Common.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationUserServices
    {

        public static void AddApplicationUserServices(this IServiceCollection services)
        {
            #region Add AspNet Core Identity

            var serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            IHostingEnvironment environment = serviceProvider.GetService<IHostingEnvironment>();

            if (environment.IsDevelopment())
            {
                services.Configure<IdentityOptions>(options =>
                {
                    //Week password settings for only development
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                });
            }

            services.AddSqlServer();


            #endregion

            #region Add AutoMapper

            services.AddSingleton(provider => new AutoMapper.MapperConfiguration(cfg =>
            {
                foreach (var profile in typeof(ApplicationUserServices).Assembly.GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AutoMapper.Profile)) && t.IsPublic)
                     .Select(t => Activator.CreateInstance(t) as AutoMapper.Profile)
                     .ToArray())
                {
                    cfg.AddProfile(profile);
                }
                cfg.ValidateInlineMaps = false;
            }).CreateMapper());

            #endregion

            services.AddScoped<IApplicationUsersUnitOfWork, ApplicationUsersUnitOfWork>();

            services.AddMessagingServices();
            services.AddServiceBusPublisher<UserRegisteredEvent>();

            services.AddFileSystemStorage();

        }

        public static void UseDataSeeding(this IApplicationBuilder app)
        {
            app.UseServiceBusPublisher<UserRegisteredEvent>();

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                //Create admin role
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var adminRole = roleManager.FindByNameAsync("admin").Result;
                if (adminRole == null)
                {
                    adminRole = new IdentityRole("admin");
                    roleManager.CreateAsync(adminRole).Wait();
                    roleManager.AddClaimAsync(adminRole, new Claim("permission", "user.view")).Wait();
                    roleManager.AddClaimAsync(adminRole, new Claim("permission", "user.create")).Wait();
                    roleManager.AddClaimAsync(adminRole, new Claim("permission", "user.modify")).Wait();
                    roleManager.AddClaimAsync(adminRole, new Claim("permission", "role.view")).Wait();
                    roleManager.AddClaimAsync(adminRole, new Claim("permission", "role.create")).Wait();
                    roleManager.AddClaimAsync(adminRole, new Claim("permission", "role.modify")).Wait();
                }

                //Create default admin user
                ApplicationDbContext dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                if (!dbContext.Users.Any())
                {
                    var newUser = new ApplicationUser()
                    {
                        UserName = "d.stojanovic@hotmail.com",
                        FirstName = "Dejan",
                        LastName = "Stojanovic",
                        Email = "d.stojanovic@hotmail.com",
                        DateOfBirth = DateTime.Parse("1984-10-31"),
                        Occupation = "IT",
                        Education = "IT",
                        EmailConfirmed = true,
                        LockoutEnabled = false
                    };

                    var task1 = userManager.CreateAsync(newUser, "Password1234!").Result;
                    var task2 = userManager.SetLockoutEnabledAsync(newUser, false).Result;

                    //Assign admin role to user
                    userManager.AddToRoleAsync(newUser, "admin").Wait();
                    
                }
            }
        }

        public static void UseDataMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }
        }
    }

}



