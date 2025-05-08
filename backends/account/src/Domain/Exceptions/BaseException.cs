using System.Diagnostics;

namespace Domain.Exceptions;
public abstract class BaseException: Exception
{
    public int StatusCode { get; }
    public string Type { get; }
    
    protected BaseException(string message, int statusCode, string type) : base(message)
    {
        Type = type;
        StatusCode = statusCode;
    }

    public object ToResponse()
    {
        return new
        {
            type = Type,
            title = Message,
            status = StatusCode,
        };
    }
}