using Contracts.Filters;

namespace WebApi;

public class SpecificHeadersFilter : RequiredHeadersAttribute
{
    public override List<HeaderDefinition> Headers =>
    [
        new("Teste", typeof(string)),
        new("TesteInt", typeof(int)),
        new("TesteEnum", typeof(MyEnum))
    ];

    public List<HeaderDefinition> GetHeaders() => Headers;
}