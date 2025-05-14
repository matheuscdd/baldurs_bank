using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net;
using Api.Filters;
using Application.Common.Interfaces.Services;
using Application.Contexts.Transactions.Commands;
using Application.Contexts.Transactions.Queries;
using Newtonsoft.Json;

namespace Api.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionController : ControllerBase 
{
    private const string QueueTransaction = "queue_transactions";
    private const string QueueAccount = "queue_accounts";
    private readonly IQueueOrchestrator _queueOrchestrator;
    public TransactionController(IQueueOrchestrator queueOrchestrator)
    {
        _queueOrchestrator = queueOrchestrator;
    }
    
    [HttpPost("regular/credit")]
    [RequiresAuth]
    public async Task<IActionResult> RegularCredit()
    {
        const string messageTypeValidation = "Account.Ensure.Owner";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var validationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeValidation, token);
        if (validationQueueResponse.Status != (int) HttpStatusCode.NoContent)
        {
            Response.StatusCode = validationQueueResponse.Status;
            return Content(validationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeRegistration = "Transaction.Credit.Regular";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeRegistration, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpPost("manager/credit")]
    [RequiresAuth]
    public async Task<IActionResult> ManagerCredit()
    {
        const string messageTypeValidation = "Account.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var validationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeValidation, token);
        if (validationQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = validationQueueResponse.Status;
            return Content(validationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeRegistration = "Transaction.Credit.Manager";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeRegistration, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpPost("regular/debit")]
    [RequiresAuth]
    public async Task<IActionResult> RegularDebit()
    {
        const string messageTypeValidation = "Account.Ensure.Owner";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var validationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeValidation, token);
        if (validationQueueResponse.Status != (int) HttpStatusCode.NoContent)
        {
            Response.StatusCode = validationQueueResponse.Status;
            return Content(validationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeRegistration = "Transaction.Debit.Regular";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeRegistration, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpPost("manager/debit")]
    [RequiresAuth]
    public async Task<IActionResult> ManagerDebit()
    {
        const string messageTypeValidation = "Account.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var validationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeValidation, token);
        if (validationQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = validationQueueResponse.Status;
            return Content(validationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeRegistration = "Transaction.Debit.Manager";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeRegistration, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }
    
    [HttpPost("regular/transfer")]
    [RequiresAuth]
    public async Task<IActionResult> RegularTransfer([FromBody] TransferCommand transferCommand)
    {
        const string messageTypeValidationOrigin = "Account.Ensure.Owner";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var bodyOrigin = JsonConvert.SerializeObject(new { AccountId = transferCommand.OriginAccountId });

        var validationOriginQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, bodyOrigin, messageTypeValidationOrigin, token);
        if (validationOriginQueueResponse.Status != (int) HttpStatusCode.NoContent)
        {
            Response.StatusCode = validationOriginQueueResponse.Status;
            return Content(validationOriginQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }
        
        const string messageTypeValidationDestination = "Account.Find.Id";
        var bodyDestination = JsonConvert.SerializeObject(new { AccountId = transferCommand.DestinationAccountId });

        var validationDestinationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, bodyDestination, messageTypeValidationDestination, token);
        if (validationDestinationQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = validationDestinationQueueResponse.Status;
            return Content(validationDestinationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        var body = JsonConvert.SerializeObject(transferCommand);
        const string messageTypeRegistration = "Transaction.Transfer.Regular";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeRegistration, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpPost("manager/transfer")]
    [RequiresAuth]
    public async Task<IActionResult> ManagerTransfer([FromBody] TransferCommand transferCommand)
    {
        const string messageTypeValidation = "Account.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var bodyOrigin = JsonConvert.SerializeObject(new { AccountId = transferCommand.OriginAccountId });

        var validationOriginQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, bodyOrigin, messageTypeValidation, token);
        if (validationOriginQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = validationOriginQueueResponse.Status;
            return Content(validationOriginQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        var bodyDestination = JsonConvert.SerializeObject(new { AccountId = transferCommand.DestinationAccountId });

        var validationDestinationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, bodyDestination, messageTypeValidation, token);
        if (validationDestinationQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = validationDestinationQueueResponse.Status;
            return Content(validationDestinationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        var body = JsonConvert.SerializeObject(transferCommand);
        const string messageTypeRegistration = "Transaction.Transfer.Manager";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeRegistration, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("regular/balance/{accountId}")]
    [RequiresAuth]
    public async Task<IActionResult> RegularBalance([FromRoute] string accountId)
    {
        const string messageTypeValidation = "Account.Ensure.Owner";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(new { AccountId = accountId });

        var validationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeValidation, token);
        if (validationQueueResponse.Status != (int) HttpStatusCode.NoContent)
        {
            Response.StatusCode = validationQueueResponse.Status;
            return Content(validationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeRegistration = "Transaction.Balance.Regular";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeRegistration, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("manager/balance/{accountId}")]
    [RequiresAuth]
    public async Task<IActionResult> ManagerBalance([FromRoute] string accountId)
    {
        const string messageTypeValidation = "Account.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(new { AccountId = accountId });

        var validationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeValidation, token);
        if (validationQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = validationQueueResponse.Status;
            return Content(validationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeRegistration = "Transaction.Balance.Manager";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeRegistration, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("manager/find/id/{transactionId}")]
    [RequiresAuth]
    public async Task<IActionResult> FindManager([FromRoute] string transactionId)
    {
        const string messageType = "Transaction.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(new { TransactionId = transactionId });

        var handleQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageType, token);

        Response.StatusCode = handleQueueResponse.Status;
        return Content(handleQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("manager/list/period")]
    [RequiresAuth]
    public async Task<IActionResult> ListPeriodManager([FromQuery] TransactionQueryParams queryParams)
    {
        const string messageTypeValidation = "Account.Find.Id";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(queryParams);

        var validationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeValidation, token);
        if (validationQueueResponse.Status != (int) HttpStatusCode.OK)
        {
            Response.StatusCode = validationQueueResponse.Status;
            return Content(validationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeTransaction = "Transaction.List.Period.Manager";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeTransaction, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

    [HttpGet("regular/list/period")]
    [RequiresAuth]
    public async Task<IActionResult> ListPeriodRegular([FromQuery] TransactionQueryParams queryParams)
    {
        const string messageTypeValidation = "Account.Ensure.Owner";
        var token = HttpContext.Items["FirebaseToken"]?.ToString();
        var body = JsonConvert.SerializeObject(queryParams);

        var validationQueueResponse = await _queueOrchestrator.HandleAsync(QueueAccount, body, messageTypeValidation, token);
        if (validationQueueResponse.Status != (int) HttpStatusCode.NoContent)
        {
            Response.StatusCode = validationQueueResponse.Status;
            return Content(validationQueueResponse.Payload!, "application/json", Encoding.UTF8);
        }

        const string messageTypeTransaction = "Transaction.List.Period.Regular";
        var registrationQueueResponse = await _queueOrchestrator.HandleAsync(QueueTransaction, body, messageTypeTransaction, token);
        Response.StatusCode = registrationQueueResponse.Status;
        return Content(registrationQueueResponse.Payload!, "application/json", Encoding.UTF8);
    }

}