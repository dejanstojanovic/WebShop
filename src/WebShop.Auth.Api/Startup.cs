using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;
using WebShop.Auth.Data;
using WebShop.Auth.Api.Configuration;
using Microsoft.AspNetCore.Identity;
using WebShop.Users.Data;
using WebShop.Users.Data.Entities;
using Microsoft.AspNetCore.Http;
using WebShop.Auth.Api.Services;
using IdentityServer4.AspNetIdentity;
using WebShop.Auth.Api.Extensions;
using Microsoft.AspNetCore.Authentication.Google;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace WebShop.Auth.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            var currentAssembly = typeof(Startup).Assembly;
            var migrationsAssembly = typeof(ConfigurationContextDesignTimeFactory).Assembly;
            byte[] certData;
            using (var resourceStream = currentAssembly.GetManifestResourceStream($"{this.GetType().Namespace}.Resources.WebShop.pfx"))
            {
                using (var memoryStream = new MemoryStream())
                {
                    resourceStream.CopyTo(memoryStream);
                    memoryStream.Flush();
                    certData = memoryStream.ToArray();
                }
            }


            services.Configure<CookiePolicyOptions>(options =>
            {
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<IUserValidator, IdentityUserValidator>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("WebShop.Users")));
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(
                    rawData: certData,
                    password: this.Configuration.GetValue<String>("OAuth:CertifiatePassword")))
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(Configuration.GetConnectionString("WebShop.Auth"),
                            sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly.GetName().Name));
                    };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(Configuration.GetConnectionString("WebShop.Auth"),
                            sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly.GetName().Name));
                    };
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityProfileService>();

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddOpenIdConnect(
                    authenticationScheme: "Google",
                    displayName: "Google",
                    configureOptions: options =>
                    {
                        Configuration.Bind("Google", options);
                        options.Scope.Add("email");
                        options.Scope.Add("profile");
                    })
                .AddMicrosoftAccount(
                    authenticationScheme: "Microsoft",
                    displayName:"Microsoft",
                    configureOptions: options =>
                    {
                        Configuration.Bind("Microsoft", options);
                        options.CorrelationCookie.SameSite = SameSiteMode.None;
                    })
                .AddFacebook(
                    authenticationScheme: "Facebook",
                    displayName: "Facebook",
                    configureOptions: options =>
                    {
                        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                        Configuration.Bind("Facebook", options);
                        options.Scope.Add("email");
                    }); 
                //.AddTwitter(
                //    authenticationScheme: "Twitter",
                //    displayName: "Twitter",
                //    configureOptions: options =>
                //    {
                //        Configuration.Bind("Twitter", options);
                //    })

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.AddDataMigration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }


       

    }
}
