using Catalog.API.Controllers;
using Catalog.Application;
using Catalog.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace Catalog.API.Swagger
{
    public class CatalogSwaggerFilter : IOperationFilter
    {
        private const string GET_NUMBER_ENUM = "D";
        private readonly string STATUSCODE_OK = HttpStatusCode.OK.ToString(GET_NUMBER_ENUM);
        private readonly string MESSAGE_BADREQUEST_FROM_MODELSTATUS = $"{HttpStatusCode.BadRequest}: Requisição inválida devido a erros do input enviado.";
        protected const string CONTENT_TYPE_JSON = "application/json";
        protected Type? CONTROLLER_TYPE = typeof(CatalogController);

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!IsFromExpectedController(context))
                return;

            var methodInfo = context.MethodInfo;
            var badRequestResponse = new OpenApiResponseBuilder(context, CONTENT_TYPE_JSON)
                                        .WithDescription(MESSAGE_BADREQUEST_FROM_MODELSTATUS)
                                        .WithModelType(typeof(ValidationProblemDetails))
                                        .Build();
            var notFound = new OpenApiResponseBuilder(context, CONTENT_TYPE_JSON)
                                    .WithDescription(HttpStatusCode.NotFound.ToString())
                                    .Build();
            var unauthorizedResponse = new OpenApiResponseBuilder(context, CONTENT_TYPE_JSON)
                                            .WithDescription(HttpStatusCode.Unauthorized.ToString())
                                            .WithModelType(typeof(ResultBase<object>))
                                            .Build();

            if (methodInfo.Name == nameof(CatalogController.GetGames) || methodInfo.Name == nameof(CatalogController.GetGamesById))
            {
                ConfigureSuccessResponseSchema(operation, context, typeof(ResultBase<GameDto>));

                operation.Responses.Add(HttpStatusCode.NotFound.ToString(GET_NUMBER_ENUM), notFound);

            }
            else if (methodInfo.Name == nameof(CatalogController.CreateGame))
            {
                operation.Summary = "Cria novo jogo";

                _ = operation.Responses.Remove(STATUSCODE_OK);

                var createdResponse = new OpenApiResponseBuilder(context, CONTENT_TYPE_JSON)
                                        .WithDescription(HttpStatusCode.Created.ToString())
                                        .WithModelType(typeof(ResultBase<GameDto>))
                                        .Build();

                operation.Responses.Add(HttpStatusCode.Unauthorized.ToString(GET_NUMBER_ENUM), unauthorizedResponse);
                operation.Responses.Add(HttpStatusCode.Created.ToString(GET_NUMBER_ENUM), createdResponse);
                operation.Responses.Add(HttpStatusCode.BadRequest.ToString(GET_NUMBER_ENUM), badRequestResponse);
                operation.Responses.Add(HttpStatusCode.NotFound.ToString(GET_NUMBER_ENUM), notFound); ;
            }
            else if (methodInfo.Name == nameof(CatalogController.UpdateGame))
            {
                operation.Summary = "Atualiza jogo";

                ConfigureSuccessResponseSchema(operation, context, typeof(ResultBase<GameDto>));

                operation.Responses.Add(HttpStatusCode.Unauthorized.ToString(GET_NUMBER_ENUM), unauthorizedResponse);
                operation.Responses.Add(HttpStatusCode.BadRequest.ToString(GET_NUMBER_ENUM), badRequestResponse);
                operation.Responses.Add(HttpStatusCode.NotFound.ToString(GET_NUMBER_ENUM), notFound);
            }
            else if (methodInfo.Name == nameof(CatalogController.BuyGame))
            {
                operation.Summary = "Compra jogo";

                _ = operation.Responses.Remove(STATUSCODE_OK);

                var accept = new OpenApiResponseBuilder(context, CONTENT_TYPE_JSON)
                                        .WithDescription(HttpStatusCode.Accepted.ToString())
                                        .WithModelType(typeof(ResultBase<object>))
                                        .Build();

                operation.Responses.Add(HttpStatusCode.Unauthorized.ToString(GET_NUMBER_ENUM), unauthorizedResponse);
                operation.Responses.Add(HttpStatusCode.Accepted.ToString(GET_NUMBER_ENUM), accept);
                operation.Responses.Add(HttpStatusCode.NotFound.ToString(GET_NUMBER_ENUM), notFound);
            }
            else if (methodInfo.Name == nameof(CatalogController.DisableGame))
            {
                operation.Summary = "Desabilita jogo";

                ConfigureSuccessResponseSchema(operation, context, typeof(ResultBase<GameDto>));

                operation.Responses.Add(HttpStatusCode.Unauthorized.ToString(GET_NUMBER_ENUM), unauthorizedResponse);
                operation.Responses.Add(HttpStatusCode.NotFound.ToString(GET_NUMBER_ENUM), notFound);
            }
        }

        private void ConfigureSuccessResponseSchema(OpenApiOperation operation, OperationFilterContext context, Type modelType)
        {
            if (operation.Responses.TryGetValue(STATUSCODE_OK, out var response))
            {
                var schema = context.SchemaGenerator.GenerateSchema(modelType, context.SchemaRepository);
                response.Content[CONTENT_TYPE_JSON] = new OpenApiMediaType { Schema = schema };
            }
        }
        private bool IsFromExpectedController(OperationFilterContext context) => context.MethodInfo?.DeclaringType?.Name == CONTROLLER_TYPE?.Name;
    }
}
