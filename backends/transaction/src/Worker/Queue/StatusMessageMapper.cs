using System.Net;

namespace Worker.Queue;

public class StatusMessageMapper
{
    private readonly Dictionary<string, HttpStatusCode> _map = new()
    {
        {"Transaction.Credit.Regular", HttpStatusCode.Created},
        {"Transaction.Credit.Manager", HttpStatusCode.Created},
        {"Transaction.Debit.Regular", HttpStatusCode.Created},
        {"Transaction.Debit.Manager", HttpStatusCode.Created},
        {"Transaction.Transfer.Regular", HttpStatusCode.Created},
        {"Transaction.Balance.Regular", HttpStatusCode.OK},
        {"Transaction.Balance.Manager", HttpStatusCode.OK},
        {"Transaction.Find.Id", HttpStatusCode.OK},
        {"Transaction.List.Period.Manager", HttpStatusCode.OK},
        {"Transaction.List.Period.Regular", HttpStatusCode.OK},
        {"Transaction.Transfer.Manager", HttpStatusCode.Created},
        {"Transaction.Has", HttpStatusCode.NoContent},
    };

    public HttpStatusCode? GetStatusCodeForType(string type) => 
        _map.TryGetValue(type, out var key) ? key : null;
}