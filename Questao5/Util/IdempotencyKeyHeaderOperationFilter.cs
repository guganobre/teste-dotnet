using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Questao5.Util
{
    public class IdempotencyKeyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Idempotency-Key",
                In = ParameterLocation.Header,
                Required = true,
                Description = "Chave de idempotência para garantir que requisições duplicadas não sejam processadas várias vezes.",
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }
    }
}