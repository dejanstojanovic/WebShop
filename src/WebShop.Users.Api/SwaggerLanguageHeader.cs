using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Api
{
    public class SwaggerLanguageHeader : IOperationFilter
    {
        readonly IServiceProvider _serviceProvider;
        public SwaggerLanguageHeader(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;          
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "Accept-Language",
                In = "header",
                Type = "string",
                Description="Supported languages",
                Enum = (_serviceProvider.GetService(typeof(IOptions<RequestLocalizationOptions>)) as IOptions<RequestLocalizationOptions>)?
                        .Value?.SupportedCultures?.Select(c=>c.TwoLetterISOLanguageName).ToList<Object>(),
                Required = false
            });
        }
    }
}
