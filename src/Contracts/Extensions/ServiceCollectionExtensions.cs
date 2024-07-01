using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddContracts(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Settings>(configuration.GetSection("Config:Virtual"));

        configuration.GetSection("Config:Virtual");
    }
}