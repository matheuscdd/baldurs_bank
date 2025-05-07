using Domain.Messaging;
using Application.Contexts.Users.Commands.Create;

namespace IoC.Messaging;

public class MessageTypeRegistry: IMessageTypeRegistry
{
    private readonly Dictionary<string, Type> _map = new()
    {
        {"User.Create", typeof(CreateUserCommand)}
    };

    private readonly Dictionary<Type, string> _reverseMap;

    public MessageTypeRegistry()
    {
        _reverseMap = _map.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
    }

    public Type? GetMessageType(string key) => 
        _map.TryGetValue(key, out var type) ? type : null;

    public string? GetKeyForType(Type type) => 
        _reverseMap.TryGetValue(type, out var key) ? key : null;

    public IEnumerable<string> GetAllKeys() => _map.Keys; 
}