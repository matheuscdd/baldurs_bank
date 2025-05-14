using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Api.Filters;
using Application.Common.Interfaces.Services;
using Application.Contexts.Accounts.Dtos;
using Newtonsoft.Json;
using Application.Contexts.Users.Dtos;

namespace Api.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase 
{
    private const string QueueAccount = "queue_accounts";
    private const string QueueTransaction = "queue_transactions";
    private const string QueueUser = "queue_users";
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

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, null, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("find/account/id/{accountId}")]
    [RequiresAuth]
    public async Task<IActionResult> FindId([FromRoute] string accountId)
    {
        const string messageType = "Account.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(new { AccountId = accountId });

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("manager/list")]
    [RequiresAuth]
    public async Task<IActionResult> List()
    {
        const string messageType = "Account.List.Manager";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, null, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("find/user/id/{accountId}")]
    [RequiresAuth]
    public async Task<IActionResult> FindUserByAccount([FromRoute] string accountId)
    {
        const string messageTypeAccount = "Account.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var bodyAccount = JsonConvert.SerializeObject(new { AccountId = accountId });

        var accountQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, bodyAccount, messageTypeAccount, token);
        if (accountQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = accountQueueResponse.Status;
            return Content(accountQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        var account = JsonConvert.DeserializeObject<AccountResponseDto>(accountQueueResponse.Payload!);
        var bodyUser = JsonConvert.SerializeObject(new { UserId = account!.UserId });
        const string messageTypeUser = "User.Find.Id";
        var userQueueResponse = await _queueOrchestrator.HandleAsync(QueueUser, bodyUser, messageTypeUser, token);
        Response.StatusCode = userQueueResponse.Status;
        var handleData = JsonConvert.DeserializeObject<UserDto>(userQueueResponse.Payload!);
        handleData!.AccountId = account!.Id;
         return Content(JsonConvert.SerializeObject(handleData), "application/json", Encoding.UTF8);
    }

    [HttpGet("find/user/number/{number:int}")]
    [RequiresAuth]
    public async Task<IActionResult> FindUserByNumber([FromRoute] int number)
    {
        const string messageTypeAccount = "Account.Find.Number";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var bodyAccount = JsonConvert.SerializeObject(new { Number = number });

        var accountQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, bodyAccount, messageTypeAccount, token);
        if (accountQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = accountQueueResponse.Status;
            return Content(accountQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        var account = JsonConvert.DeserializeObject<AccountResponseDto>(accountQueueResponse.Payload!);
        var bodyUser = JsonConvert.SerializeObject(new { UserId = account!.UserId });
        const string messageTypeUser = "User.Find.Id";
        var userQueueResponse = await _queueOrchestrator.HandleAsync(QueueUser, bodyUser, messageTypeUser, token);
        Response.StatusCode = userQueueResponse.Status;
        var handleData = JsonConvert.DeserializeObject<UserDto>(userQueueResponse.Payload!);
        handleData!.AccountId = account!.Id;
        return Content(JsonConvert.SerializeObject(handleData), "application/json", Encoding.UTF8);
    }

    [HttpDelete("manager/remove/id/{accountId}")]
    [RequiresAuth]
    public async Task<IActionResult> RemoveAccountByManager([FromRoute] string accountId)
    {

        const string messageTypeAccount = "Account.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(new { AccountId = accountId });

        var accountQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeAccount, token);
        if (accountQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = accountQueueResponse.Status;
            return Content(accountQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeTransaction = "Transaction.Has";
        var transactionQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeTransaction, token);
        string? messageTypeDynamic;

        if (transactionQueueResponse.Status == (int) HttpStatusCode.NotFound)
        {
            messageTypeDynamic = "Account.Delete.Manager";
        }
        else if (transactionQueueResponse.Status == (int) HttpStatusCode.NoContent)
        {
            messageTypeDynamic = "Account.Disable.Manager";
        }
        else
        {
            Response.StatusCode = transactionQueueResponse.Status;
            return Content(transactionQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        accountQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeDynamic, token);
        Response.StatusCode = accountQueueResponse.Status;
        return Content(accountQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpDelete("regular/remove/id/{accountId}")]
    [RequiresAuth]
    public async Task<IActionResult> RemoveAccountByRegular([FromRoute] string accountId)
    {
        const string messageTypeAccount = "Account.Ensure.Owner";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(new { AccountId = accountId });

        var accountQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeAccount, token);
        
        if (accountQueueResponse.Status != (int) HttpStatusCode.NoContent)
        {
            Response.StatusCode = accountQueueResponse.Status;
            return Content(accountQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeTransaction = "Transaction.Has";
        var transactionQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeTransaction, token);
        string? messageTypeDynamic;
        if (transactionQueueResponse.Status == (int) HttpStatusCode.NotFound)
        {
            messageTypeDynamic = "Account.Delete.Regular";
        }
        else if (transactionQueueResponse.Status == (int) HttpStatusCode.NoContent)
        {
            messageTypeDynamic = "Account.Disable.Regular";
        }
        else
        {
            Response.StatusCode = transactionQueueResponse.Status;
            return Content(transactionQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        accountQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeDynamic, token);
        Response.StatusCode = accountQueueResponse.Status;
        return Content(accountQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }
}