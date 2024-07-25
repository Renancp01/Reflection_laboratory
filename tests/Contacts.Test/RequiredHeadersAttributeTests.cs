using Contracts.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;

namespace Contacts.Test;

public class RequiredHeadersAttributeTests
{
    private readonly RequiredHeadersAttribute _attribute;

    //public RequiredHeadersAttributeTests()
    //{
    //    _attribute = new RequiredHeadersAttribute();
    //}
    [Fact]
    public async Task OnActionExecutionAsync_AllHeadersPresent_ShouldProceed()
    {
        // Arrange
        var headers = new HeaderDictionary
        {
            { "StringHeader", "test" },
            { "IntHeader", "123" },
            { "EnumHeader", "Value1" }
        };
        var context = CreateActionExecutingContext(headers);

        var next = new Mock<ActionExecutionDelegate>();
        next.Setup(n => n()).ReturnsAsync(new ActionExecutedContext(context, new List<IFilterMetadata>(), new Mock<ControllerBase>().Object));

        // Act
        await _attribute.OnActionExecutionAsync(context, next.Object);

        // Assert
        next.Verify(n => n(), Times.Exactly(3));
        //Assert.Null(context.Result);
    }
    [Fact]
    public async Task OnActionExecutionAsync_MissingHeaders_ShouldReturnBadRequest()
    {
        var headers = new HeaderDictionary
        {
            { "StringHeader", "test" }
        };
        var context = CreateActionExecutingContext(headers);

        var next = new Mock<ActionExecutionDelegate>();

        await _attribute.OnActionExecutionAsync(context, next.Object);

        next.Verify(n => n(), Times.Never);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(context.Result);
        var errors = badRequestResult.Value as dynamic;
        //Assert.Contains(errors.Errors, e => e.Header == "IntHeader" && e.Error == "Missing");
        //Assert.Contains(errors.Errors, e => e.Header == "EnumHeader" && e.Error == "Missing");
    }

    [Fact]
    public async Task OnActionExecutionAsync_InvalidHeaders_ShouldReturnBadRequest()
    {
        var headers = new HeaderDictionary
        {
            { "StringHeader", "test" },
            { "IntHeader", "invalid-int" },
            { "EnumHeader", "invalid-enum" }
        };
        var context = CreateActionExecutingContext(headers);

        var next = new Mock<ActionExecutionDelegate>();

        await _attribute.OnActionExecutionAsync(context, next.Object);

        next.Verify(n => n(), Times.Never);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(context.Result);
        var errors = badRequestResult.Value as dynamic;
        //Assert.Contains(errors.Errors, e => e.Header == "IntHeader" && e.Error == "Invalid Value");
        //Assert.Contains(errors.Errors, e => e.Header == "EnumHeader" && e.Error == "Invalid Value");
    }

    private ActionExecutingContext CreateActionExecutingContext(IHeaderDictionary headers)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Clear();
        foreach (var header in headers)
        {
            httpContext.Request.Headers.Add(header.Key, header.Value);
        }

        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new Mock<ControllerBase>().Object);

        return actionExecutingContext;
    }
}