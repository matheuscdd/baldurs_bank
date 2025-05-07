using System.Net;

namespace Domain.Exceptions;

public class UnauthorizedCustomException : BaseException
{
    public UnauthorizedCustomException(string message = "Unauthorized") 
        : base(
            message, 
            (int) HttpStatusCode.Unauthorized,
            "https://tools.ietf.org/html/rfc9110#section-15.5.2"
          ) { }
}