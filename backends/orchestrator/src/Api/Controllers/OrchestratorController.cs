using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Application.Common.Dtos;
using System.Text;

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
        var messageType = "User.Create";
        var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized();
        }
        
        var idToken = authHeader.Substring("Bearer ".Length).Trim();

        try 
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var rpcClient = new RpcClient();
            await rpcClient.StartAsync();
            var rawQueueResponse = await rpcClient.CallAsync(
                JsonConvert.SerializeObject(
                    new {
                        MessageType = messageType,
                        IdToken = idToken,
                        Payload = JsonConvert.DeserializeObject<object>(body),
                    }
                )
            );
            var handleQueueResponse = JsonConvert.DeserializeObject<QueueResponseDto>(rawQueueResponse);
            handleQueueResponse.Payload = Encoding.UTF8.GetString(Convert.FromBase64String(handleQueueResponse.Payload));
            
            Response.StatusCode = handleQueueResponse.Status;
            return Content(handleQueueResponse.Payload, "application/json", Encoding.UTF8);
        } 
        catch(Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}