using System;
using System.Threading.Tasks;
using WebShop.Users.Api.Extensions;
using WebShop.Users.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace WebShop.Users.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
            Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
        }



        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiServices();
            services.AddApplicationUserServices();
            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = this.Configuration.GetValue<String>("OpenIdConnect:Authority");
                options.Audience = this.Configuration.GetValue<String>("OpenIdConnect:Audience");
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetValue<String>("OpenIdConnect:Audience"),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<String>("OpenIdConnect:Authority"),
                    ValidateLifetime = true
                };
                options.RequireHttpsMetadata = false;
            });

            #region Authorization rules
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SameUserOrAdmin", policy =>
                {
                    policy.AddRequirements(new SameUserAuthReqirement());
                });
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireRole("admin");
                });
                options.AddPolicy("RoleAdmin", policy =>
                {
                    policy.RequireClaim("permission", new[] { "role.add", "role.modify", "role.view" });
                });
                options.AddPolicy("UserAdmin", policy =>
                {
                    policy.RequireClaim("permission", new[] { "user.add", "user.modify", "user.view" });
                });
            });

            services.AddSingleton<IAuthorizationHandler, SameUserAuthHandler>();

            #endregion

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddSwaggerApiDocumentation();
            services.AddMvc().AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    //Point to assembly with embeded resources
                    var assemblyName = new AssemblyName(typeof(ApplicationUserServices).GetTypeInfo().Assembly.FullName);
                    return factory.Create("Translations", assemblyName.Name);
                };
            }); ;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="applicationLifetime"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(OnShutdown);
            if (env.IsDevelopment())
            {
                app.UseDataMigrations();
                app.UseDataSeeding();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSwaggerApiDocumentation();
            app.UseApiServices();

            app.UseMvc();
        }

        private void OnShutdown()
        {
            //Do the clean up if required
        }
    }
}
