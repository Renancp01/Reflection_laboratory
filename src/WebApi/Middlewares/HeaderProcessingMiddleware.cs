namespace WebApi.Middlewares;

public class HeaderProcessingMiddleware
{
    private readonly RequestDelegate _next;

    public HeaderProcessingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("YourHeaderName", out var headerValue))
        {
            // Process the header value as needed
            context.Items["ProcessedHeader"] = headerValue.ToString();
            await _next(context);
        }
        else
        {
            // Return a 400 Bad Request response if the header is not present
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Required header 'YourHeaderName' is missing.");
        }
    }
}