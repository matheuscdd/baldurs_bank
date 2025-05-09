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

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(body, messageType, null);

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

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(body, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("list")]
    [RequiresAuth]
    public async Task<IActionResult> List()
    {
        const string messageType = "User.List";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(null, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("find/{id}")]
    [RequiresAuth]
    public async Task<IActionResult> Find([FromRoute] string id)
    {
        const string messageType = "User.Find";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(new { Id = id });

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(body, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }
}