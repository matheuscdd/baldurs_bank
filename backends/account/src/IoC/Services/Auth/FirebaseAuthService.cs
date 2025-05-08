using Application.Common.Interfaces.Auth;
using FirebaseAdmin.Auth;

namespace IoC.Services.Auth;

public class FirebaseAuthService : IAuthService
{
    public async Task<FirebaseToken?> ValidateTokenAsync(string token)
    {
        try
        {
            var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            return decoded;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}