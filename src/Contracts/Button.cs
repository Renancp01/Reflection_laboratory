using Contracts.Extensions;

namespace Contracts;

public record Button : ILinkableItem
{
    public string WidgetKey { get; set; }

    public Link Link { get; set; }
}