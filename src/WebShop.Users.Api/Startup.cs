﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Users.Api.Extensions;
using WebShop.Users.Common.Extensions;
using WebShop.Users.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebShop.Common.Extensions;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace WebShop.Users.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get;  }

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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserIdPolicy", policy => policy.RequireClaim("userid"));

                options.AddPolicy("PermissionCreatePolicy", policy =>
                {
                    policy.RequireClaim("permission", "create");
                });
                options.AddPolicy("PermissionViewPolicy", policy =>
                {
                    policy.RequireClaim("permission", "view");
                });
                options.AddPolicy("PermissionModifyPolicy", policy =>
                {
                    policy.RequireClaim("permission", "update");
                });
            });

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
