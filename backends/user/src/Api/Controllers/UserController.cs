using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase 
{
    public UserController()
    {

    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(new Dictionary<string, string>{{"testing", "123"}});
    }
}