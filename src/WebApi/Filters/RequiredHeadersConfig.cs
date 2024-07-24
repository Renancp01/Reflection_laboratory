using System;

namespace WebApi.Filters;

public static class RequiredHeadersConfig
{
    public static readonly List<HeaderDefinition> Headers =
    [
        new HeaderDefinition("Teste", typeof(string)),
        new HeaderDefinition("TesteInt", typeof(int)),
        new HeaderDefinition("TesteEnum", typeof(MyEnum))
    ];
}

public class HeaderDefinition(string name, Type type)
{
    public string Name { get; } = name;
    
    public Type Type { get; } = type;
}

public enum MyEnum
{
    Value1,
    Value2,
    Value3
}