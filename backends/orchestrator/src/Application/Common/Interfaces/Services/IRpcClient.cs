namespace Application.Common.Interfaces.Services;

public interface IRpcClient : IAsyncDisposable
{
    Task StartAsync();
    Task<string> CallAsync(string queue_name, string message, CancellationToken cancellationToken = default);
}
