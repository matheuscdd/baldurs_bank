using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Application.Common.Dtos;
using System.Text;
using Domain.Messaging;
using Api.Filters;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class OrchestratorController : ControllerBase 
{
    public OrchestratorController()
    {
    }

    [HttpGet("user/middle")]
    [RequiresAuth]
    public async Task<IActionResult> Read()
    {
        var messageType = "User.Create";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();

        try 
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var rpcClient = new RpcClient();
            await rpcClient.StartAsync();
            var rawQueueResponse = await rpcClient.CallAsync(
                JsonConvert.SerializeObject(
                    new Envelope{
                            MessageType = messageType,
                            Token = token,
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