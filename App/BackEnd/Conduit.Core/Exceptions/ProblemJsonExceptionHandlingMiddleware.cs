using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Core.Exceptions
{
    public static class ProblemJsonExceptionHandlingMiddleware
    {
        public static void UseProblemDetailsGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        var problemDetails = new ProblemDetails
                        {
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                            Title = "Internal Server Error",
                            Detail = contextFeature.Error.Message,
                            Status = context.Response.StatusCode
                        };
                        
                        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                    }
                });
            });
        }   
    }
}