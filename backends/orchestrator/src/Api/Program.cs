using Api.Filters;
using IoC.Dependencies;
using Api.Exceptions;

var builder = WebApplication.CreateBuilder(args);
var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? throw new Exception("RABBITMQ_HOST cannot be empty");
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? throw new Exception("RABBITMQ_USER cannot be empty");
var rabbitMqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? throw new Exception("RABBITMQ_PASSWORD cannot be empty");
builder.Configuration["RabbitMQ:HostName"] = rabbitMqHost;
builder.Configuration["RabbitMQ:UserName"] = rabbitMqUser;
builder.Configuration["RabbitMQ:Password"] = rabbitMqPassword;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<TokenValidationFilter>();
});

builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseCors("DevCors");
app.UseCustomExceptionHandling();
app.UseStatusCodePages();
app.MapControllers();

await app.RunAsync();
