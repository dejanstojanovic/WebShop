using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebShop.Common.Serialization;
using WebShop.Common.Extensions;
using IdentityModel.Client;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace WebShop.Web.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.ConfigureJsonSettings();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });

            services.AddHttpClient();
            services.AddTransient<AutomaticCookieTokenEvents>();
            services.AddTransient<TokenEndpointService>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.Cookie.Expiration = TimeSpan.FromHours(1);
                options.EventsType = typeof(AutomaticCookieTokenEvents);
            })
              .AddOpenIdConnect(options =>
              {
                  Configuration.Bind("OpenIdConnect", options);
                  options.Scope.Add("webshop.users.api");
                  options.Scope.Add("offline_access");

                  options.Events = new OpenIdConnectEvents
                  {
                      OnRemoteFailure = context => {
                          var ex = context.Failure;

                          context.Response.Redirect("/");
                          context.HandleResponse();

                          return Task.FromResult(0);
                      }
                  };

              });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<HttpClient>(provider => {
                var httpContextAccess = provider.GetService<IHttpContextAccessor>();
                var httpClient = new HttpClient();
                var accessToken = httpContextAccess.HttpContext.GetTokenAsync("access_token").Result;
                var refreshToken = httpContextAccess.HttpContext.GetTokenAsync("refresh_token").Result;
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                return httpClient;

            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
        }
    }
}
