using System.Collections.Concurrent;

namespace Application.Common.Interfaces.Services;

public interface IRpcClient : IAsyncDisposable
{
    Task StartAsync();
    Task<string> CallAsync(string message, CancellationToken cancellationToken = default);
}
