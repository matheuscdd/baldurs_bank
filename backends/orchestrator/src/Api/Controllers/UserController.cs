using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net;
using Api.Filters;
using Application.Common.Interfaces.Services;
using Newtonsoft.Json;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase 
{
    private const string QueueUser = "queue_users";
    private readonly IQueueOrchestrator _queueOrchestrator;
    public UserController(IQueueOrchestrator queueOrchestrator)
    {
        _queueOrchestrator = queueOrchestrator;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate()
    {
        const string messageType = "User.Validate";
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(QueueUser, body, messageType, null);

        if (handleQueueResponse.Status == (int) HttpStatusCode.NoContent)
        {
            return NoContent();
        }
        
        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpPost("create")]
    [RequiresAuth]
    public async Task<IActionResult> Create()
    {
        const string messageType = "User.Create";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(QueueUser, body, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("manager/list")]
    [RequiresAuth]
    public async Task<IActionResult> List()
    {
        const string messageType = "User.List.Manager";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(QueueUser, null, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("find/id/{userId}")]
    [RequiresAuth]
    public async Task<IActionResult> Find([FromRoute] string userId)
    {
        const string messageType = "User.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(new { UserId = userId });

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(QueueUser, body, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }
}