using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Contracts.Filters;
using WebApi.Filters;

namespace WebApi;

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var methodInfo = context.MethodInfo;

        var requiredHeadersAttribute = methodInfo.GetCustomAttribute<RequiredHeadersAttribute>();
        if (requiredHeadersAttribute is null)
            return;

        var requiredHeaders = RequiredHeadersConfig.Headers;

        operation.Parameters ??= new List<OpenApiParameter>();

        foreach (var header in requiredHeaders)
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
