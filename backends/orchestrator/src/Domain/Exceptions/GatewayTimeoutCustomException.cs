using System.Net;

namespace Domain.Exceptions;

public class GatewayTimeoutCustomException : BaseException
{
    public GatewayTimeoutCustomException(string message = "Gateway Timeout") 
        : base(
            message, 
            (int) HttpStatusCode.GatewayTimeout, 
            "https://tools.ietf.org/html/rfc9110#section-15.5.9"
        ) { }
}