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
        Console.WriteLine("chegou");
        try 
        {
            var rpcClient = new RpcClient();
            await rpcClient.StartAsync();
            var response = await rpcClient.CallAsync("15");
            Console.WriteLine(response);
            return Ok(new Dictionary<string, string>{{"enviado", response}});
        } 
        catch(Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}