using Application.Common.Interfaces.Services;
using Application.Common.Dtos;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Domain.Messaging;

namespace Application.Services;

public class QueueOrchestrator: IQueueOrchestrator
{
    private readonly IRpcClient _rpcClient;

    public QueueOrchestrator(IRpcClient rpcClient)
    {
        _rpcClient = rpcClient;
    }

    public async Task<QueueResponseDto> HandleAsync(string body, string messageType, string? token)
    {
        try
        {
            var envelope = new Envelope
            {
                Token = token,
                MessageType = messageType,
                Payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(body))
            };

            await _rpcClient.StartAsync();

            var rawQueueResponse = await _rpcClient.CallAsync(JsonConvert.SerializeObject(envelope));
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
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao processar a requisição", ex);
        }
    }
}