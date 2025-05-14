using IoC.Dependencies;

var builder = WebApplication.CreateBuilder(args);
var postgresUrl = Environment.GetEnvironmentVariable("PGSQL_URL_TRANSACTIONS") ?? throw new Exception("PGSQL_URL_TRANSACTIONS cannot be empty");
var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? throw new Exception("RABBITMQ_HOST cannot be empty");
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? throw new Exception("RABBITMQ_USER cannot be empty");
var rabbitMqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? throw new Exception("RABBITMQ_PASSWORD cannot be empty");
builder.Configuration["RabbitMQ:HostName"] = rabbitMqHost;
builder.Configuration["RabbitMQ:UserName"] = rabbitMqUser;
builder.Configuration["RabbitMQ:Password"] = rabbitMqPassword;
builder.Configuration["ConnectionStrings:DefaultConnection"] = postgresUrl;

builder.Services.AddInfrastructure(builder.Configuration);
    
var app = builder.Build();

await app.RunAsync();
