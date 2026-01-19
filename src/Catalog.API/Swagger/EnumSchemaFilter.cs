using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Catalog.API.Swagger
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var names = Enum.GetNames(context.Type);
                var values = Enum.GetValues(context.Type).Cast<byte>();

                schema.Enum.Clear();
                foreach (var (name, value) in names.Zip(values))
                    schema.Enum.Add(new OpenApiString($"{name} = {value}"));

                schema.Type = "string";
                schema.Format = null;
            }
        }
    }
}
