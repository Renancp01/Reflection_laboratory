using Microsoft.AspNetCore.Mvc.Filters;

namespace Contracts.Filters;

public interface IRequiredHeadersFilter : IAsyncActionFilter
{
    List<HeaderDefinition> Headers { get; }
}