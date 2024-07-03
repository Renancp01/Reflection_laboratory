using Contracts;
using Microsoft.Extensions.Options;

namespace WebApi;

public class ConfigurationHolder
{
    public Settings SomeSetting { get; set; }

    public ConfigurationHolder(IOptions<Settings> settings)
    {
        SomeSetting = settings.Value;
    }
}