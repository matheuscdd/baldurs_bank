using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces.Auth;
using IoC.Services.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;


namespace IoC.Dependencies;

public static class FirebaseSetup
{
    public static void AddFirebase(this IServiceCollection services, IConfiguration configuration)
    {
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(configuration["Firebase:CredentialPath"]!)
        });

        services.AddSingleton<IAuthService, FirebaseAuthService>();
    }
}
