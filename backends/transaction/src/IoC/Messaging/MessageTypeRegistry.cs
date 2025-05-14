using Domain.Messaging;
using Application.Contexts.Transactions.Commands.CreditAccountRegular;
using Application.Contexts.Transactions.Commands.DebitAccountRegular;
using Application.Contexts.Transactions.Commands.TransferAccountRegular;
using Application.Contexts.Transactions.Commands.TransferAccountManager;
using Application.Contexts.Transactions.Queries.GetBalanceRegular;
using Application.Contexts.Transactions.Queries.GetTransactionById;
using Application.Contexts.Transactions.Queries.GetTransactionsManagerByPeriod;
using Application.Contexts.Transactions.Queries.GetTransactionsRegularByPeriod;
using Application.Contexts.Transactions.Queries.GetBalanceManager;
using Application.Contexts.Transactions.Commands.DebitAccountManager;
using Application.Contexts.Transactions.Commands.CreditAccountManager;
using Application.Contexts.Transactions.Queries.CheckHasTransactionsManager;

namespace IoC.Messaging;

public class MessageTypeRegistry: IMessageTypeRegistry
{
    private readonly Dictionary<string, Type> _map = new()
    {
        {"Transaction.Credit.Regular", typeof(CreditAccountRegularCommand)},
        {"Transaction.Credit.Manager", typeof(CreditAccountManagerCommand)},
        {"Transaction.Debit.Regular", typeof(DebitAccountRegularCommand)},
        {"Transaction.Debit.Manager", typeof(DebitAccountManagerCommand)},
        {"Transaction.Transfer.Regular", typeof(TransferAccountRegularCommand)},
        {"Transaction.Balance.Regular", typeof(GetBalanceRegularQuery)},
        {"Transaction.Balance.Manager", typeof(GetBalanceManagerQuery)},
        {"Transaction.Find.Id", typeof(GetTransactionByIdQuery)},
        {"Transaction.List.Period.Manager", typeof(GetTransactionsManagerByPeriodQuery)},
        {"Transaction.List.Period.Regular", typeof(GetTransactionsRegularByPeriodQuery)},
        {"Transaction.Transfer.Manager", typeof(TransferAccountManagerCommand)},
        {"Transaction.Has", typeof(CheckHasTransactionsQuery)},
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