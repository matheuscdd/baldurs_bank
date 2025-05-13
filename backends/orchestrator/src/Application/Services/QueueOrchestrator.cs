using Application.Common.Interfaces.Services;
using Application.Common.Dtos;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Domain.Messaging;
using Domain.Exceptions;

namespace Application.Services;

public class QueueOrchestrator: IQueueOrchestrator
{
    private readonly IRpcClient _rpcClient;

    public QueueOrchestrator(IRpcClient rpcClient)
    {
        _rpcClient = rpcClient;
    }

    public async Task<QueueResponseDto> HandleAsync(string queue_name, string? body, string messageType, string? token)
    {
        var envelope = new Envelope
        {
            Token = token,
            MessageType = messageType,
            Payload = string.IsNullOrEmpty(body) ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(body))
        };

        await _rpcClient.StartAsync();
        
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        var cancellationToken = cts.Token;

        try
        {
            var rawQueueResponse = await _rpcClient.CallAsync(queue_name, JsonConvert.SerializeObject(envelope), cancellationToken);
            var handleQueueResponse = JsonConvert.DeserializeObject<QueueResponseDto>(rawQueueResponse);

            if (handleQueueResponse.Status == (int) HttpStatusCode.NoContent || string.IsNullOrEmpty(handleQueueResponse.Payload))
            {
                handleQueueResponse.Payload = null;
            }
            else
            {
                handleQueueResponse.Payload = Encoding.UTF8.GetString(Convert.FromBase64String(handleQueueResponse.Payload));
            }

            return handleQueueResponse;
        }
        catch (OperationCanceledException)
        {
            throw new GatewayTimeoutCustomException();
        }
    }
}