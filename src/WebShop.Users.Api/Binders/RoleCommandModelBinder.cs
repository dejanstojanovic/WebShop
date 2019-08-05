using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebShop.Common.Extensions;

namespace WebShop.Users.Api
{
    public class RoleCommandModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var routeNameValue = bindingContext.ActionContext.RouteData.Values["roleName"] as String;
            routeNameValue = routeNameValue ?? bindingContext.ActionContext.RouteData.Values["name"] as String;

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
            var idProperty = modelType.GetProperties().FirstOrDefault(p => p.Name.Equals("roleName", StringComparison.InvariantCultureIgnoreCase) || p.Name.Equals("roleName", StringComparison.InvariantCultureIgnoreCase));
            if (idProperty != null)
            {
                idProperty.ForceSetValue(result, routeNameValue);
            }

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
