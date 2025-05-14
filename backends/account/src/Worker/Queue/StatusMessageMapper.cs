using System.Net;

namespace Worker.Queue;

public class StatusMessageMapper
{
    private readonly Dictionary<string, HttpStatusCode> _map = new()
    {
        {"Account.Create", HttpStatusCode.Created},
        {"Account.Ensure.Owner", HttpStatusCode.NoContent},
        {"Account.Find.Id", HttpStatusCode.OK},
        {"Account.Find.Number", HttpStatusCode.OK},
        {"Account.List.Manager", HttpStatusCode.OK},
        {"Account.Disable.Manager", HttpStatusCode.NoContent},
        {"Account.Disable.Regular", HttpStatusCode.NoContent},
        {"Account.Delete.Regular", HttpStatusCode.NoContent},
        {"Account.Delete.Manager", HttpStatusCode.NoContent},
    };

    public HttpStatusCode? GetStatusCodeForType(string type) => 
        _map.TryGetValue(type, out var key) ? key : null;
}