using Contracts.Extensions;

namespace Contracts;

public class Shortcut : ILinkableItem
{
    public Guid WidgetKey => Guid.NewGuid();

    public Icon Icon { get; set; }

    public Link Link { get; set; }
}