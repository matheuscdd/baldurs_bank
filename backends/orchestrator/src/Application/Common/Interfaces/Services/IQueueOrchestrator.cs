using Application.Common.Dtos;

namespace Application.Common.Interfaces.Services;

public interface IQueueOrchestrator
{
    Task<QueueResponseDto> HandleAsync(string? requestBody, string messageType, string? token);
}