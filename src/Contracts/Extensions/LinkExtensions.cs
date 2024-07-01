namespace Contracts.Extensions;

public static class LinkExtensions
{
    private static void FillParamsFromSource(this Link link, object source)
    {
        var properties = source.GetType().GetProperties();
        foreach (var property in properties)
        {
            var propName = property.Name;
            var propValue = property.GetValue(source);

            if (link != null && link.Params.ContainsKey(propName) &&
                string.IsNullOrEmpty(link.Params[propName]?.ToString()))
                link.Params[propName] = propValue;
        }
    }

    public static void FillParamsFromSourceList<T>(this IEnumerable<T> items, object source) where T : ILinkableItem
    {
        foreach (var item in items)
        {
            if (item is null)
                return;

            item.Link.FillParamsFromSource(source);
        }
    }
}