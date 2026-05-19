using Microsoft.AspNetCore.Diagnostics;
using ProjectManagement.Application.Common;
using System.Net;
using System.Text.Json;

namespace ProjectManagement.API.Middlewares
{
    public static  class ExceptionMiddleware
    {
        // Centralized Error Handling
        // Captures all unhandled exceptions in logs

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, Serilog.ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (contextFeature != null)
                    {
                        var ex = contextFeature.Error;
                        logger.Error(ex, "Unhandled exception occurred");
                        var response = new ApiResponse<string>
                        {
                            Success = false,
                            ErrorMessages = new List<string>
                            {
                                 "Something went wrong"
                            },
                            StatusCode = context.Response.StatusCode
                        };
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }

    }
}
