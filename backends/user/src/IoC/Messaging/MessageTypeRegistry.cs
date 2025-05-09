using Domain.Messaging;
using Application.Contexts.Users.Commands.Create;
using Application.Contexts.Users.Commands.Validate;
using Application.Contexts.Users.Queries.GetAll;
using Application.Contexts.Users.Queries.GetById;

namespace IoC.Messaging;

public class MessageTypeRegistry: IMessageTypeRegistry
{
    private readonly Dictionary<string, Type> _map = new()
    {
        {"User.Create", typeof(CreateUserCommand)},
        {"User.Validate", typeof(ValidateUserCommand)},
        {"User.List", typeof(GetAllUserQuery)},
        {"User.Find", typeof(GetByIdUserQuery)},
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