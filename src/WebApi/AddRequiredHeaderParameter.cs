using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Contracts.Filters;

namespace WebApi;

public class AddRequiredHeaderParameter : IOperationFilter
{
    private readonly IServiceProvider _serviceProvider;

    public AddRequiredHeaderParameter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        using var scope = _serviceProvider.CreateScope();
        var headerFilters = scope.ServiceProvider.GetServices<IRequiredHeadersFilter>();

        operation.Parameters ??= new List<OpenApiParameter>();

        foreach (var headerFilter in headerFilters)
        {
            foreach (var header in headerFilter.Headers)
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
