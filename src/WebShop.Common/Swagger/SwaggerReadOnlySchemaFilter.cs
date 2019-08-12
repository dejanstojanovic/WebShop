using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Common.Swagger
{
    public class SwaggerReadOnlySchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Ignore readonly properties of the CQRS commands
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            schema.ReadOnly = false;
            if (schema.Properties != null)
            {
                foreach (var keyValuePair in schema.Properties)
                {
                    keyValuePair.Value.ReadOnly = false;
                }
            }
        }
    }
}
