using System.Net;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Exceptions;

public static class AppExceptions {
  public static WebApplication UseCustomExceptionHandling(this WebApplication app) {
    app.UseStatusCodePages();
    app.UseExceptionHandler(config => {
      config.Run(async context => {
        var logger = context.RequestServices.GetRequiredService<ILogger<WebApplicationBuilder>>();

        context.Response.ContentType = "application/json";
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (ex is BaseException custom) {
          context.Response.StatusCode = custom.StatusCode;
          logger.LogError(ex, $"Handle exception: {ex.Message}");
        
          await context.Response.WriteAsJsonAsync(custom.ToResponse());
        } else {
          context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
          logger.LogError(ex, $"Unhandled exception: {ex.Message}");
        
          await context.Response.WriteAsJsonAsync(new InternalServerCustomException().ToResponse());
        }
      });
    });

    return app;
  }
}