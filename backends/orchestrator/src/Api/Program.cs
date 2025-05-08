using Api.Filters;
using IoC.Dependencies;
using Api.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<TokenValidationFilter>();
});

builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseCustomExceptionHandling();
app.UseStatusCodePages();
app.MapControllers();

await app.RunAsync();
