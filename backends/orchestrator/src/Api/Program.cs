using Api.Filters;
using IoC.Dependencies;
using Api.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors",
        policy => policy
        // TODO - trocar pelo docker (adaptar - prod angular)
            .AllowAnyOrigin() // ou 
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
