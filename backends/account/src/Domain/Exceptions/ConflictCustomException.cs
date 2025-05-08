using System.Net;

namespace Domain.Exceptions;

public class ConflictCustomException : BaseException
{
    public ConflictCustomException(string message = "Conflict") 
        : base(
            message, 
            (int) HttpStatusCode.Conflict, 
            "https://tools.ietf.org/html/rfc9110#section-15.5.10"
        ) { }
}