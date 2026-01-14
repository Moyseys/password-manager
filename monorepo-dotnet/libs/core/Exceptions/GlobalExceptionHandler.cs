using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ExceptionDetailBuilder detail = CreateExceptionDetail(exception);

        await BuildResponse(detail, httpContext, cancellationToken);

        if(detail.Status == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Unhandled exception occurred: {ExceptionType} - {Message}", exception.GetType().Name, exception.Message);
        
        return true; //Indica que o erro foi tratado
    }

    private ExceptionDetailBuilder CreateExceptionDetail(Exception exception)
    {
        return exception switch
        {
            InvalidDataException => new ExceptionDetailBuilder()
                    .SetStatus(StatusCodes.Status400BadRequest)
                    .SetDetail(exception.Message)
                    .SetTitle("Bad request")
                    .Build(),
            UnauthorizedAccessException => new ExceptionDetailBuilder()
                    .SetStatus(StatusCodes.Status401Unauthorized)
                    .SetDetail(exception.Message)
                    .SetTitle("Unauthorized")
                    .Build(),
            KeyNotFoundException => new ExceptionDetailBuilder()
                    .SetStatus(StatusCodes.Status404NotFound)
                    .SetDetail(exception.Message)
                    .SetTitle("Not Found")
                    .Build(),
            ConflictException => new ExceptionDetailBuilder()
                    .SetStatus(StatusCodes.Status409Conflict)
                    .SetDetail(exception.Message)
                    .SetTitle("Conflict")
                    .Build(),
            _ => new ExceptionDetailBuilder()
        };
    } 

    private static async Task BuildResponse(ExceptionDetailBuilder exceptionDetail, HttpContext httpContext, CancellationToken cancellation)
    {
        httpContext.Response.StatusCode = exceptionDetail.Status;
        await httpContext.Response.WriteAsJsonAsync(exceptionDetail, cancellation);
    }
}