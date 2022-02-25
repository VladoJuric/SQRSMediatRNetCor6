using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebUI.OpenApi
{
    public class ModifyEnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (schema.Enum.Count > 0)
            {
                var names = Enum.GetNames(type);
                var openApiEnumNames = new OpenApiArray();
                foreach (var name in names)
                {
                    openApiEnumNames.Add(new OpenApiString(name));
                }
                schema.Extensions.Add("x-enum-varnames", openApiEnumNames);
            }
        }
    }
}
