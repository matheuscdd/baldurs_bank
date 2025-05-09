using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net;
using Api.Filters;
using Application.Common.Interfaces.Services;
using Newtonsoft.Json;

namespace Api.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase 
{
    private readonly IQueueOrchestrator _queueOrchestrator;
    public AccountController(IQueueOrchestrator queueOrchestrator)
    {
        _queueOrchestrator = queueOrchestrator;
    }
    
    [HttpPost("create")]
    [RequiresAuth]
    public async Task<IActionResult> Create()
    {
        const string messageType = "Account.Create";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(null, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

}