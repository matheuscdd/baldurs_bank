using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using System.Threading.Tasks;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase 
{
    public UserController()
    {

    }

    public async Task<FirebaseToken?> VerifyFirebaseToken(string idToken)
    {
        try
        {
            var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            return decoded;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    [HttpGet]
    public async Task<IActionResult> MinhaAcao()
    {
        var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var idToken = authHeader.Substring("Bearer ".Length).Trim();
            // Agora você pode usar o token
            
            var meta = await VerifyFirebaseToken(idToken);
            if (meta is not null)
            {
                var claims = new Dictionary<string, object>
                {
                    { "isManager", true }
                };
                // TODO - na criação do usuário preciso setar essa role, a partir de então todos os login futuros já terão a role
                // Depois só preciso verificar as claims para definir as permissões
                await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(meta.Uid, claims);
                return Ok(new { meta });
            }
        }

        return Unauthorized();
    }
}