using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Repositories.Users;
using Application.Contexts.Users.Repositories;
using Domain.Messaging;
using IoC.Messaging;
using Mapster;
using Microsoft.Extensions.Logging;
using Worker.Queue;
using Application.Services;

namespace IoC.Dependencies;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information)
        );
        
        services.AddSingleton<IMessageTypeRegistry, MessageTypeRegistry>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IQueueConsumer, QueueConsumer>(); 
        services.AddHostedService<RpcQueueWorker>();
        
        services.AddMapster();
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Application.AssemblyReference
                .GetAssembly());
        });
        
        services.AddFirebase(configuration);
        
        return services;
    }
}