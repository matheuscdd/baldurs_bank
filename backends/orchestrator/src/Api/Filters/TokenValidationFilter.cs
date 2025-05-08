using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;


namespace Api.Filters;

public class TokenValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var hasAuthAttribute = context.ActionDescriptor.EndpointMetadata
            .OfType<RequiresAuthAttribute>()
            .Any();

        if (!hasAuthAttribute)
        {
            await next();
            return;
        }

        var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        context.HttpContext.Items["FirebaseToken"] = token;

        await next();
    }
}
