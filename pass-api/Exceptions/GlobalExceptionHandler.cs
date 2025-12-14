using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace PasswordManager.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ExceptionDetailBuilder detail = CreateExceptionDetail(exception);

        if(detail != null) await BuildResponse(detail, httpContext, cancellationToken);

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
            SecurityTokenExpiredException => new ExceptionDetailBuilder()
                    .SetStatus(StatusCodes.Status401Unauthorized)
                    .SetDetail("Your authentication token has expired. Please sign in again to continue.")
                    .SetTitle("Unauthorized - Token Expired")
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
            _ => new ExceptionDetailBuilder()
        };
    } 

    private static async Task BuildResponse(ExceptionDetailBuilder exceptionDetail, HttpContext httpContext, CancellationToken cancellation)
    {
        httpContext.Response.StatusCode = exceptionDetail.Status;
        await httpContext.Response.WriteAsJsonAsync(exceptionDetail, cancellation);
    }
}