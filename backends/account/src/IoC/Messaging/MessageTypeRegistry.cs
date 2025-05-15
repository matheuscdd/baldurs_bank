using Application.Contexts.Accounts.Commands.CreateManager;
using Application.Contexts.Accounts.Commands.DeleteRegular;
using Application.Contexts.Accounts.Commands.DeleteManager;
using Application.Contexts.Accounts.Commands.DisableManager;
using Application.Contexts.Accounts.Commands.DisableRegular;
using Application.Contexts.Accounts.Queries.EnsureAccountOwner;
using Application.Contexts.Accounts.Queries.GetAccountById;
using Application.Contexts.Accounts.Queries.GetAccountByNumber;
using Application.Contexts.Accounts.Queries.GetAccountByUser;
using Application.Contexts.Accounts.Queries.GetAllAccount;
using Domain.Messaging;
using Application.Contexts.Accounts.Commands.CreateRegular;

namespace IoC.Messaging;

public class MessageTypeRegistry: IMessageTypeRegistry
{
    private readonly Dictionary<string, Type> _map = new()
    {
        {"Account.Create.Regular", typeof(CreateRegularAccountCommand)},
        {"Account.Create.Manager", typeof(CreateManagerAccountCommand)},
        {"Account.Ensure.Owner", typeof(EnsureAccountOwnerQuery)},
        {"Account.Find.Id", typeof(GetAccountByIdQuery)},
        {"Account.Find.Number", typeof(GetAccountByNumberQuery)},
        {"Account.Find.User", typeof(GetAccountByUserQuery)},
        {"Account.List.Manager", typeof(GetAllAccountQuery)},
        {"Account.Disable.Manager", typeof(DisableManagerAccountCommand)},
        {"Account.Disable.Regular", typeof(DisableRegularAccountCommand)},
        {"Account.Delete.Regular", typeof(DeleteRegularAccountCommand)},
        {"Account.Delete.Manager", typeof(DeleteManagerAccountCommand)}
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