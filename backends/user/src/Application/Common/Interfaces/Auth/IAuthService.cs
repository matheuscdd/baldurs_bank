using FirebaseAdmin.Auth;

namespace Application.Common.Interfaces.Auth;

public interface IAuthService
{
    Task<FirebaseToken?> ValidateTokenAsync(string token);
}
