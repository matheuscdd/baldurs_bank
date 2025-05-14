namespace Domain.Messaging;

public interface IMessageTypeRegistry
{
    Type? GetMessageType(string key);
    string? GetKeyForType(Type type);
    IEnumerable<string> GetAllKeys();
}