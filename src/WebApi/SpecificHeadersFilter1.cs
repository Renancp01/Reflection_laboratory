using Contracts.Filters;

namespace WebApi;

public class SpecificHeadersFilter1 : RequiredHeadersFilterBase
{
    public override List<HeaderDefinition> Headers =>
    [
        new("Teste1", typeof(string)),
    ];
}