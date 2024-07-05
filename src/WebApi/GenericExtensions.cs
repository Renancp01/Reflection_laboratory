using System;
using System.Collections.Generic;

public static class GenericExtensions
{
    public static void Process<T, TItem, TConfig>(this T obj, Func<TItem, TConfig> configProvider) where T : class
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var properties = obj.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(List<TItem>))
            {
                var items = property.GetValue(obj) as List<TItem>;
                ProcessItems(items, configProvider);
            }
        }
    }

    private static void ProcessItems<TItem, TConfig>(IEnumerable<TItem> items, Func<TItem, TConfig> configProvider)
    {
        if (items == null) return;

        foreach (var item in items)
        {
            var config = configProvider(item);
            ApplyConfig(item, config);
        }
    }

    private static void ApplyConfig<TItem, TConfig>(TItem item, TConfig config)
    {
        var itemProperties = item.GetType().GetProperties();
        var configProperties = config.GetType().GetProperties();

        foreach (var configProp in configProperties)
        {
            var itemProp = Array.Find(itemProperties, p => p.Name == configProp.Name);
            if (itemProp != null && itemProp.CanWrite)
            {
                var value = configProp.GetValue(config);
                itemProp.SetValue(item, value);
            }
        }
    }
}