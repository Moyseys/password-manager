using Microsoft.AspNetCore.Diagnostics;

namespace PasswordManager.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case InvalidDataException: 
                var detail = new ExceptionDetailBuilder()
                    .SetStatus(StatusCodes.Status400BadRequest)
                    .SetDetail(exception.Message)
                    .SetTitle("Bad request")
                    .Build();

                await BuildResponse(detail, httpContext, cancellationToken);
                break;
            default:
                await httpContext.Response.WriteAsJsonAsync(new ExceptionDetailBuilder(), cancellationToken);
            break;
        }

        return true; //Indica que o erro foi tratado
    }

    private static async Task BuildResponse(ExceptionDetailBuilder exceptionDetail, HttpContext httpContext, CancellationToken cancellation)
    {
        httpContext.Response.StatusCode = exceptionDetail.Status;
        await httpContext.Response.WriteAsJsonAsync(exceptionDetail, cancellation);
    }
}