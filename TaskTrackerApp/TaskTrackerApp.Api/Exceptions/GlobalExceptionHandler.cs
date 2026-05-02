using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TaskTrackerApp.Api.Exceptions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred",
            Instance = context.Request.Path,
            Detail = exception.Message
        };

        switch (exception)
        {
            case OperationCanceledException when context.RequestAborted.IsCancellationRequested:
                problemDetails.Title = "Request canceled by client";
                problemDetails.Status = StatusCodes.Status499ClientClosedRequest;
                break;

            case OperationCanceledException:
                problemDetails.Title = "Request timeout";
                problemDetails.Status = StatusCodes.Status408RequestTimeout;
                break;

            default:
                problemDetails.Status = StatusCodes.Status500InternalServerError;

                if (context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
                {
                    problemDetails.Extensions["stackTrace"] = exception.StackTrace;
                }
                break;
        }

        context.Response.StatusCode = problemDetails.Status.Value;
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}