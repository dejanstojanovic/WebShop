using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Sample.Api;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebShop.Users.Api.Extensions
{
    /// <summary>
    /// Swagger startup configuration
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtensions
    {

        /// <summary>
        /// Adds Swagger serice to IoC container
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerApiDocumentation(this IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddJsonFormatters()
                    .AddVersionedApiExplorer(
                      options =>
                      {
                          options.GroupNameFormat = "'v'VVV";
                          // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                          // can also be used to control the format of the API version in route templates
                          options.SubstituteApiVersionInUrl = true;
                      });

            services.AddApiVersioning(options => options.ReportApiVersions = true);

            #region NSwag
            Array.ForEach(services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions.ToArray(),
                (description) =>
                {
                    services.AddSwaggerDocument(document =>
                    {
                        document.DocumentName = description.GroupName;
                    });
                });
            #endregion

            services.AddSwaggerGen(
                options =>
                {
                    // Resolve the temprary IApiVersionDescriptionProvider service
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    String assemblyDescription = typeof(Startup).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

                    // Add a swagger document for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, new Swashbuckle.AspNetCore.Swagger.Info()
                        {
                            Title = $"{typeof(Startup).Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product} {description.ApiVersion}",
                            Version = description.ApiVersion.ToString(),
                            Description = description.IsDeprecated ? $"{assemblyDescription} - DEPRECATED" : $"{assemblyDescription}" +
                            $"<p><img src='/swagger-ui/healthcheck-30x30.png' valign='middle'/><a href='/health' target='_blank'>Healthchecks</a></p>" +
                            $"<p><img src='/swagger-ui/rocket-30x30.png' valign='middle'/>Build #{typeof(Startup).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}</p>"
                        });
                    }

                    // Add a custom filter for settint the default values
                    options.OperationFilter<SwaggerDefaultValues>();
                    options.OperationFilter<SwaggerLanguageHeader>();
                    options.OperationFilter<SwaggerFileOperationFilter>();
                    options.SchemaFilter<SwaggerReadOnlySchemaFilter>();

                    // Tells swagger to pick up the output XML document files
                    var currentAssembly = Assembly.GetExecutingAssembly();
                    var xmlDocs = currentAssembly.GetReferencedAssemblies()
                    .Union(new AssemblyName[] { currentAssembly.GetName() })
                    .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                    .Where(f => File.Exists(f)).ToArray();

                    Array.ForEach(xmlDocs, (d) =>
                    {
                        options.IncludeXmlComments(d);
                    });

                    

                    #region Authorization
                    // Add authorization option
                    var security = new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer", new string[] { }},
                    };

                    options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });
                    options.AddSecurityRequirement(security);
                    #endregion

                });

        }

        /// <summary>
        /// Adds Swagger middleware to the pipeline with all custom settings
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerApiDocumentation(this IApplicationBuilder app)
        {
            #region Configure Swagger
            Microsoft.AspNetCore.Builder.SwaggerBuilderExtensions.UseSwagger(app);
            //app.UseSwagger();
            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            app.UseSwaggerUI(
                options =>
                {
                    //options.InjectStylesheet(@"/swagger-ui/theme-material.css");
                    //options.InjectStylesheet(@"/swagger-ui/ui.css");
                    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);

                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });

            #endregion
        }


    }
}
