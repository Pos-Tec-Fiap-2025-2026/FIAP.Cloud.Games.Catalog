using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Catalog.API.Swagger
{
    internal class OpenApiResponseBuilder
    {
        private readonly string _contentType;
        private string _description = string.Empty;
        private readonly OperationFilterContext _context;
        private Type _modelType = typeof(object);
        public OpenApiResponseBuilder(OperationFilterContext context, string contentType)
        {
            _context = context;
            _contentType = contentType;
        }

        public OpenApiResponseBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public OpenApiResponseBuilder WithModelType(Type modelType)
        {
            _modelType = modelType;
            return this;
        }

        public OpenApiResponse Build()
        {
            var response = new OpenApiResponse
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [_contentType] = new OpenApiMediaType
                    {
                        Schema = _context.SchemaGenerator.GenerateSchema(_modelType, _context.SchemaRepository)
                    }
                }
            };

            if (!string.IsNullOrEmpty(_description))
                response.Description = _description;

            return response;
        }
    }
}
