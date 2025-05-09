using System.Net;

namespace Worker.Queue;

public class StatusMessageMapper
{
    private readonly Dictionary<string, HttpStatusCode> _map = new()
    {
        {"Account.Create", HttpStatusCode.Created},
    };

    public HttpStatusCode? GetStatusCodeForType(string type) => 
        _map.TryGetValue(type, out var key) ? key : null;
}