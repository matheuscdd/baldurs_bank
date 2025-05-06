using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class OrchestratorController : ControllerBase 
{
    public OrchestratorController()
    {

    }

    [HttpGet("user/middle")]
    public async Task<IActionResult> Read()
    {
        var rpc = await RpcClient.InitAsync();
        await rpc.CallAsync("abc");
        return Ok(new Dictionary<string, string>{{"ta", "read"}});
    }
}