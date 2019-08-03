using WebShop.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using WebShop.Users.Common.Dtos;

namespace WebShop.Users.Api.Extensions
{
    /// <summary>
    /// ASP.NET Core infrastructure startup
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ApiServiceExtensions
    {
        /// <summary>
        /// Adds ASP.NET Core infrastructure services 
        /// </summary>
        /// <param name="services"></param>
        public static void AddApiServices(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            IHostingEnvironment hostingEnvironment = serviceProvider.GetService<IHostingEnvironment>();

            #region Caching
            services.AddDistributedMemoryCache();
            #endregion

            #region Logging
            services.AddLogging();
            #endregion

            #region Add healthchecks
            services.AddHealthChecks()
                .AddSqlServer(connectionString: configuration.GetConnectionString("WebShop.Users"), name: "SqlServer")
                .AddUrlGroup(uri: new Uri($"{configuration.GetValue<String>("OpenIdConnect:Authority")}/.well-known/openid-configuration"), name: "AuthServer");
            #endregion

            #region Add localization
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("sr"),
                        new CultureInfo("nl")
                    };
                    opts.DefaultRequestCulture = new RequestCulture("en", "en");
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });
            #endregion

            #region Add Cors
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.WithOrigins(configuration.GetSection("Cors:Origins").Get<String[]>()));
                c.AddPolicy("AllowAnyOrigin", options => options.AllowAnyOrigin());
            });
            #endregion

        }


        /// <summary>
        /// Adds ASP.NET Core infrastructure middlewares to the pipeline 
        /// </summary>
        /// <param name="app"></param>
        public static void UseApiServices(this IApplicationBuilder app)
        {
            #region Logging
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();
            app.Use(async (httpContext, next) =>
            {
                var username = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "anonymous";
                LogContext.PushProperty("User", username);
                await next.Invoke();
            });          
            loggerFactory.AddSerilog();
            #endregion

            #region Exception handle
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;
                var result = JsonConvert.SerializeObject(new ErrorMessage(exception));
                context.Response.ContentType = "application/json";

                if(exception is NotFoundException)
                    context.Response.StatusCode = StatusCodes.Status404NotFound;

                if (exception is DuplicateException)
                    context.Response.StatusCode = StatusCodes.Status409Conflict;

                if(exception is MismatchException)
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsync(result);
            }));
            #endregion

            #region Localization
            app.UseRequestLocalization();
            #endregion

            #region Healthchecks
            var healthcheckOptions = new HealthCheckOptions();
            healthcheckOptions.ResultStatusCodes[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable;

            healthcheckOptions.ResponseWriter = async (ctx, rpt) =>
            {
                var result = JsonConvert.SerializeObject(new
                {
                    Status = rpt.Status.ToString(),
                    Errors = rpt.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                }, Formatting.None, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                ctx.Response.ContentType = MediaTypeNames.Application.Json;
                await ctx.Response.WriteAsync(result);
            };

            app.UseHealthChecks("/health", healthcheckOptions);

            #endregion
        }
    }
}
