using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase 
{
    public UserController()
    {

    }

    [HttpGet("send")]
    public async Task<IActionResult> Send()
    {
        await new RabbitMqService().Send("Hello");
        return Ok(new Dictionary<string, string>{{"testing", "123"}});
    }

    [HttpGet("read")]
    public async Task<IActionResult> Read()
    {
        await new RabbitMqService().Read();
        return Ok(new Dictionary<string, string>{{"ta", "read"}});
    }
}