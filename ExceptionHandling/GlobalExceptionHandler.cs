using Microsoft.AspNetCore.Diagnostics;
using StajApi.Exceptions;
using StajApi.Models;

namespace StajApi.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        (int statusCode, string message) = exception switch
        {
            BadRequestException => (
                StatusCodes.Status400BadRequest,
                exception.Message
            ),
            UnauthorizedException => (
                StatusCodes.Status401Unauthorized,
                exception.Message
            ),
            NotFoundException => (
                StatusCodes.Status404NotFound,
                exception.Message
            ),
            ConflictException => (
                StatusCodes.Status409Conflict,
                exception.Message
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Beklenmeyen bir sunucu hatası oluştu."
            )
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new ErrorResponse { Message = message },
            cancellationToken
        );

        return true;
    }
}
