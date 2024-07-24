using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Filters;

public class RequiredHeadersAttribute : Attribute, IAsyncActionFilter
{
    private static readonly Dictionary<Type, Func<string, (bool IsValid, object Value)>> Validators = new()
    {
        { typeof(string), value => (true, value) },
        { typeof(int), value => (int.TryParse(value, out var intValue), intValue) },
        { typeof(MyEnum), value => (Enum.TryParse(typeof(MyEnum), value, true, out var enumValue), enumValue) }
    };

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var missingHeaders = new List<string>();
        var invalidHeaders = new List<string>();

        foreach (var header in RequiredHeadersConfig.Headers)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(header.Name, out var headerValue))
            {
                missingHeaders.Add(header.Name);
            }
            else if (!Validators.TryGetValue(header.Type, out var validator))
            {
                invalidHeaders.Add(header.Name);
            }
            else
            {
                var (isValid, parsedValue) = validator(headerValue);
                if (isValid)
                {
                    context.HttpContext.Items[header.Name] = parsedValue;
                }
                else
                {
                    invalidHeaders.Add(header.Name);
                }
            }
        }

        if (missingHeaders.Count != 0 || invalidHeaders.Count != 0)
        {
            var errorResponse = new
            {
                MissingHeaders = missingHeaders,
                InvalidHeaders = invalidHeaders
            };
            context.Result = new BadRequestObjectResult(errorResponse);
            return;
        }

        await next();
    }
}