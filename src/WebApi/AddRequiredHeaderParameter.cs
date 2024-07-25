using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;

namespace WebApi;

public class AddRequiredHeaderParameter(IOptions<SpecificHeadersFilter> headerConfig) : IOperationFilter
{
    private readonly SpecificHeadersFilter _headerConfig = headerConfig.Value;

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        
        foreach (var header in _headerConfig.Headers)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = header.Name,
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = GetOpenApiType(header.Type)
                }
            });
        }
    }
    private static string GetOpenApiType(Type type)
    {
        if (type == typeof(string))
        {
            return "string";
        }
        if (type == typeof(int))
        {
            return "integer";
        }
        if (type.IsEnum)
        {
            return "string";
        }

        throw new NotSupportedException($"Type '{type.Name}' is not supported.");
    }
}
