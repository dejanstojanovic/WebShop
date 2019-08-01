using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Users.Api
{
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId == "PostImage")
            {
                operation.Parameters = new List<IParameter>
                {
                    new NonBodyParameter
                    {
                        Name = "photo",
                        Required = true,
                        Type = "file",
                        In = "formData"
                    }
                };
            }

            //if (operation.OperationId == "GetImage" || 
            //    operation.OperationId == "Image")
            //{
            //    operation.Responses = new Dictionary<String, Response>() {
            //        { "200",
            //        new Response() {
            //            Description="OK",
            //            Schema = new Schema() { Format = "file" }
            //            }
            //        }
            //    };
            //}
        }
    }
}
