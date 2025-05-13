using System.Net;

namespace Domain.Exceptions;

public class ValidationCustomException : BaseException
{
    public ValidationCustomException(string message = "Validation Error") 
        : base(
            message, 
            (int) HttpStatusCode.BadRequest,
            "https://tools.ietf.org/html/rfc9110#section-15.5.1"
          ) { }
}