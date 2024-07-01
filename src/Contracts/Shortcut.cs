using Contracts.Extensions;

namespace Contracts;

public record Shortcut : ILinkableItem
{
    public string WidgetKey { get; set; }

    public Icon Icon { get; set; }

    public Link Link { get; set; }
}