using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebShop.Common.Extensions;

namespace WebShop.Users.Api
{
    public class UserCommandModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var routeIdStringValue = bindingContext.ActionContext.RouteData.Values["userId"] as String;
            routeIdStringValue = routeIdStringValue ?? bindingContext.ActionContext.RouteData.Values["id"] as String;

            String valueFromBody;
            using (var streamReader = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                valueFromBody = streamReader.ReadToEnd();
            }
            if (string.IsNullOrEmpty(valueFromBody))
            {
                return Task.CompletedTask;
            }
            var modelType = bindingContext.ModelMetadata.UnderlyingOrModelType;
            var result = JsonConvert.DeserializeObject(valueFromBody, modelType);

            if (!String.IsNullOrWhiteSpace(routeIdStringValue) && Guid.TryParse(routeIdStringValue, out var routeIdValue))
            {
                var idProperty = modelType.GetProperties().FirstOrDefault(p => p.Name.Equals("userId", StringComparison.InvariantCultureIgnoreCase) || p.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase));
                if (idProperty != null)
                {
                    idProperty.ForceSetValue(result, routeIdValue);
                }
            }

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
