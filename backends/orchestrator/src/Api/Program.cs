using Api.Controllers;
using RabbitMQ.Client;
using Api.Filters;
using IoC.Dependencies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<TokenValidationFilter>();
});


builder.Services.AddMessagingDependencyInjection();

var app = builder.Build();


app.UseExceptionHandler();
app.UseStatusCodePages();
app.MapControllers();

await app.RunAsync();
