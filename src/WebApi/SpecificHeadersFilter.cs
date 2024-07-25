using Contracts.Filters;

namespace WebApi;

public class SpecificHeadersFilter : RequiredHeadersFilterBase
{
    public override List<HeaderDefinition> Headers =>
    [
        new("Teste", typeof(string)),
        new("TesteInt", typeof(int)),
        new("TesteEnum", typeof(MyEnum))
    ];
}