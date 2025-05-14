using System.Net;

namespace Worker.Queue;

public class StatusMessageMapper
{
    private readonly Dictionary<string, HttpStatusCode> _map = new()
    {
        {"User.Create", HttpStatusCode.Created},
        {"User.Validate", HttpStatusCode.NoContent},
        {"User.List", HttpStatusCode.OK},
        {"User.Find.Id", HttpStatusCode.OK},
    };

    public HttpStatusCode? GetStatusCodeForType(string type) => 
        _map.TryGetValue(type, out var key) ? key : null;
}