using Microsoft.Extensions.DependencyInjection;

namespace IoC.Dependencies;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMessagingDependencyInjection();
        return services;
    }
}