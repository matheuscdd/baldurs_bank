using Api.Controllers;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Repository.Repositories.Users;
using Application.Contexts.Users.Repositories;
using Domain.Messaging;
using IoC.Messaging;
using Mapster;
using Worker.Queue;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information)
);

builder.Services.AddMapster();
builder.Services.AddMediatR(options =>
    {
        options.RegisterServicesFromAssembly(Application.AssemblyReference
            .GetAssembly());
    });

builder.Services.AddSingleton<IMessageTypeRegistry, MessageTypeRegistry>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<QueueConsumer>(); 
builder.Services.AddHostedService<RpcQueueWorker>();

var firebasePath = builder.Configuration["Firebase:CredentialPath"];
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(firebasePath),
});
    
var app = builder.Build();

app.MapControllers();

app.Run();
