using IoC.Dependencies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
    
var app = builder.Build();

await app.RunAsync();