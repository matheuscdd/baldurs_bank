using System.Net;

namespace Domain.Exceptions;

public class InternalServerCustomException : BaseException
{
    public InternalServerCustomException(string message = "Internal Server Error") 
        : base(
            message, 
            (int) HttpStatusCode.InternalServerError, 
            "https://tools.ietf.org/html/rfc9110#section-15.6.1"
        ) { }
}