using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase 
{
    public UserController()
    {

    }

    [HttpGet("read")]
    public async Task<IActionResult> Read()
    {
        return Ok(new Dictionary<string, string>{{"lendo", "fila"}});
    }
}