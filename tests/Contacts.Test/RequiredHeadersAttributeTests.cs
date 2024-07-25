using Contracts.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class RequiredHeadersFilterBaseTests
{
    private class TestRequiredHeadersFilter : RequiredHeadersFilterBase
    {
        public override List<HeaderDefinition> Headers { get; } = new List<HeaderDefinition>
        {
            new HeaderDefinition("X-Test-Header", typeof(string) ),
            new HeaderDefinition( "X-Test-Int-Header", typeof(int) )
        };
    }

    [Fact]
    public async Task OnActionExecutionAsync_AllHeadersPresentAndValid_ContinuesExecution()
    {
        var context = new ActionExecutingContext(
            new ActionContext(new DefaultHttpContext(), new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object());

        context.HttpContext.Request.Headers["X-Test-Header"] = "HeaderValue";
        context.HttpContext.Request.Headers["X-Test-Int-Header"] = "123";

        var next = new Mock<ActionExecutionDelegate>();
        next.Setup(n => n()).Returns(Task.FromResult(new ActionExecutedContext(context, new List<IFilterMetadata>(), new object())));

        var filter = new TestRequiredHeadersFilter();
        await filter.OnActionExecutionAsync(context, next.Object);

        next.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task OnActionExecutionAsync_MissingHeader_ReturnsBadRequest()
    {
        var context = new ActionExecutingContext(
            new ActionContext(new DefaultHttpContext(), new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object());

        context.HttpContext.Request.Headers["X-Test-Header"] = "HeaderValue";

        var next = new Mock<ActionExecutionDelegate>();

        var filter = new TestRequiredHeadersFilter();
        await filter.OnActionExecutionAsync(context, next.Object);

        Assert.IsType<BadRequestObjectResult>(context.Result);
    }

    [Fact]
    public async Task OnActionExecutionAsync_InvalidHeaderValue_ReturnsBadRequest()
    {
        var context = new ActionExecutingContext(
            new ActionContext(new DefaultHttpContext(), new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object());

        context.HttpContext.Request.Headers["X-Test-Header"] = "HeaderValue";
        context.HttpContext.Request.Headers["X-Test-Int-Header"] = "InvalidInt";

        var next = new Mock<ActionExecutionDelegate>();

        var filter = new TestRequiredHeadersFilter();
        await filter.OnActionExecutionAsync(context, next.Object);

        Assert.IsType<BadRequestObjectResult>(context.Result);
    }

    [Fact]
    public async Task OnActionExecutionAsync_InvalidHeaderType_ReturnsBadRequest()
    {
        var context = new ActionExecutingContext(
            new ActionContext(new DefaultHttpContext(), new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object());

        context.HttpContext.Request.Headers["X-Test-Header"] = "HeaderValue";
        context.HttpContext.Request.Headers["X-Test-Int-Header"] = "123";

        var next = new Mock<ActionExecutionDelegate>();

        var filter = new TestRequiredHeadersFilter();
        await filter.OnActionExecutionAsync(context, next.Object);

        Assert.IsType<BadRequestObjectResult>(context.Result);
    }
}