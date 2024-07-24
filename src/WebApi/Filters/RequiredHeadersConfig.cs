using System;

namespace WebApi.Filters;

public static class RequiredHeadersConfig
{
    public static readonly List<HeaderDefinition> Headers = new List<HeaderDefinition>
    {
        new("Teste", typeof(string)),
        new("TesteInt", typeof(int)),
        new("TesteEnum", typeof(MyEnum))
    };
}

public class HeaderDefinition
{
    public string Name { get; }
    public Type Type { get; }

    public HeaderDefinition(string name, Type type)
    {
        Name = name;
        Type = type;
    }
}


public enum MyEnum
{
    Value1,
    Value2,
    Value3
}