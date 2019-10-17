using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace ERP
{
    /// <summary>
    /// swagger配置 在header里增加令牌auth字段
    /// </summary>
    public class SwaggerHttpHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();
            operation.parameters.Add(new Parameter { name = "auth", @in = "header", description = "令牌", required = false, type = "string" });
        }
    }
}