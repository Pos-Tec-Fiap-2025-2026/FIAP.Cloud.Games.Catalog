using Microsoft.OpenApi.Models;

namespace Catalog.API.Swagger
{
    internal static class SwaggerExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Fiap Cloud Games User",
                    Version = "v1",
                    Description = "<b>API que expõe endpoints REST </b><br/><br/>"
                });
                options.OperationFilter<CatalogSwaggerFilter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Insira o token JWT assim: Bearer {seu token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });


                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.SchemaFilter<EnumSchemaFilter>();
            });
        }

        public static void AddUseSwagger(this WebApplication? app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
                options.DocumentTitle = "Documentação da API";
                options.RoutePrefix = "swagger";
            });
        }
    }
}
