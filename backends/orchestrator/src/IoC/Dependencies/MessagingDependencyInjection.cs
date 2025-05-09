using Application.Common.Interfaces.Services;
using IoC.Communication.Queue;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Dependencies;

public static class MessagingDependencyInjection
{
    public static IServiceCollection AddMessagingDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IRpcClient, RpcClient>();
        services.AddScoped<IQueueOrchestrator, QueueOrchestrator>();
        return services;
    }
}