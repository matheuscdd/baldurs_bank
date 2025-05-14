namespace Application.Services;

public interface IQueueConsumer
{
    public Task<(object, int)> OnMessageReceived(string rawJson);
}