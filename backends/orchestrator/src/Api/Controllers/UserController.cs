using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net;
using Domain.Messaging;
using Api.Filters;
using Application.Common.Interfaces.Services;

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
        try 
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var handleQueueResponse = await _queueOrchestrator.HandleAsync(body, messageType, null);

            if (handleQueueResponse.Status == (int) HttpStatusCode.NoContent)
            {
                return NoContent();
            }
            
            Response.StatusCode = handleQueueResponse.Status;
            return Content(handleQueueResponse.Payload, "application/json", Encoding.UTF8);
        } 
        catch(Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpPost("create")]
    [RequiresAuth]
    public async Task<IActionResult> Create()
    {
        const string messageType = "User.Create";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        try 
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var handleQueueResponse = await _queueOrchestrator.HandleAsync(body, messageType, token);

            Response.StatusCode = handleQueueResponse.Status;
            return Content(handleQueueResponse.Payload, "application/json", Encoding.UTF8);
        } 
        catch(Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}