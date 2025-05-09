using System.Net;

namespace Domain.Exceptions;

public class NotFoundCustomException : BaseException
{
    public NotFoundCustomException(string message = "Not Found") 
        : base(
            message, 
            (int) HttpStatusCode.NotFound,
            "https://tools.ietf.org/html/rfc9110#section-15.5.5"
          ) { }
}