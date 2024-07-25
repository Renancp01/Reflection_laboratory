using Contracts.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Contracts.Filters;

[AttributeUsage(AttributeTargets.All)]
public abstract class RequiredHeadersFilterBase : Attribute, IRequiredHeadersFilter
{
    public abstract List<HeaderDefinition> Headers { get; }

    private static readonly Dictionary<Type, Func<string, (bool IsValid, object Value)>> Validators = new()
    {
        { typeof(string), value => (true, value) },
        { typeof(int), value => (int.TryParse(value, out var intValue), intValue) },
        { typeof(MyEnum), value => (Enum.TryParse(typeof(MyEnum), value, true, out var enumValue), enumValue) }
    };

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var missingOrInvalidHeaders = Headers
            .Select(header => ValidateHeader(context, header))
            .Where(result => result != null)
            .ToList();

        if (missingOrInvalidHeaders.Any())
        {
            var errorResponse = new
            {
                Errors = missingOrInvalidHeaders
            };
            context.Result = new BadRequestObjectResult(errorResponse);
            return;
        }

        await next();
    }

    private static object ValidateHeader(ActionExecutingContext context, HeaderDefinition header)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(header.Name, out var headerValue))
        {
            //return /*new { Header = header.Name, Error = "Missing" };*/

            return new Error("BadRequest", "");
        }

        if (!Validators.TryGetValue(header.Type, out var validator))
        {
            return new { Header = header.Name, Error = "Invalid Type" };
        }

        var (isValid, parsedValue) = validator(headerValue);
        if (!isValid)
        {
            return new { Header = header.Name, Error = "Invalid Value" };
        }

        context.HttpContext.Items[header.Name] = parsedValue;
        return null;
    }


}
