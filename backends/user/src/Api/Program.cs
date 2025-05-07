using Api.Controllers;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information)
);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<Worker>();

// await Host.CreateDefaultBuilder(args)
//     .ConfigureServices(services =>
//     {
//         services.AddHostedService<Worker>();
//     })
//     .Build()
//     .RunAsync();

var firebasePath = builder.Configuration["Firebase:CredentialPath"];
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(firebasePath),
});
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseExceptionHandler();
app.UseStatusCodePages();
app.MapControllers();

app.Run();
