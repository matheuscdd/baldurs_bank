using MediatR;
using Newtonsoft.Json;
using Domain.Messaging;
using IoC.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces.Auth;
using Domain.Exceptions;
using System.Text;

namespace Worker.Queue;

public class QueueConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMessageTypeRegistry _registry;
    private readonly IAuthService _authService;

    public QueueConsumer(
        IServiceScopeFactory scopeFactory,
        IMessageTypeRegistry registry,
        IAuthService authService
    )
    {
        _scopeFactory = scopeFactory;
        _registry = registry;
        _authService = authService;
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

        if (!string.IsNullOrEmpty(envelope.Payload))
        {
            envelope.Payload = Encoding.UTF8.GetString(Convert.FromBase64String(envelope.Payload));
        }

        var request = string.IsNullOrEmpty(envelope.Payload) ? 
                Activator.CreateInstance(type)! : 
                JsonConvert.DeserializeObject(envelope.Payload, type); 
        if (request == null) 
        {
            throw new InvalidOperationException($"Failed to deserialize payload to {type.Name}");
        }
        
        if (typeof(IRequireAuth).IsAssignableFrom(type) || typeof(IRequireManager).IsAssignableFrom(type))
        {
            if (string.IsNullOrEmpty(envelope.Token))
            {
                throw new UnauthorizedCustomException("Empty token");
            }

            var firebaseToken = await _authService.ValidateTokenAsync(envelope.Token);
            if (firebaseToken is null) 
            {
                throw new UnauthorizedCustomException();
            }

            if (request is IRequireAuth authRequest)
            {
                authRequest.TokenId = firebaseToken.Uid;
                authRequest.TokenEmail = firebaseToken.Claims["email"].ToString();
            }

            firebaseToken.Claims.TryGetValue("isManager", out object isManager);
            if (request is IRequireManager && isManager is not true)
            {
                throw new UnauthorizedCustomException("Only managers can access");
            }
        }

        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var response = await mediator.Send(request);
        var statusCode = (new StatusMessageMapper()).GetStatusCodeForType(envelope.MessageType);
        return (response, (int) statusCode);
    }
}
