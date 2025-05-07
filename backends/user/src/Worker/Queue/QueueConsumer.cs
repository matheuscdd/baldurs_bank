using MediatR;
using Newtonsoft.Json;
using Domain.Messaging;
using IoC.Messaging;
using Application.Contexts.Users.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Worker.Queue;

public class QueueConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMessageTypeRegistry _registry;

    public QueueConsumer(
        IServiceScopeFactory scopeFactory,
        IMessageTypeRegistry registry
    )
    {
        _scopeFactory = scopeFactory;
        _registry = registry;
    }

    public async Task<(object, int)> OnMessageReceived(string rawJson)
    {
        var envelope = JsonConvert.DeserializeObject<Envelope>(rawJson);
        if (envelope == null || string.IsNullOrEmpty(envelope.MessageType)) {
            throw new InvalidOperationException("Mensagem inválida ou sem tipo definido.");
        }

        var type = _registry.GetMessageType(envelope.MessageType);
        if (type is null) {
            throw new InvalidOperationException($"Unrecognized message type: {envelope.MessageType}");
        }

        if (!typeof(IRequest<UserDto>).IsAssignableFrom(type)) {
            throw new InvalidOperationException($"O tipo '{type.FullName}' não implementa IRequest<UserDto>");
        }

        var request = envelope.Payload.ToObject(type);
        if (request == null) {
            throw new InvalidOperationException($"Failed to deserialize payload to {type.Name}");
        }

        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var response = await mediator.Send(request);
        var statusCode = (new StatusMessageMapper()).GetStatusCodeForType(envelope.MessageType);
        return (response, (int) statusCode);
    }
}
