using System.Net;
using System.Text.Json;
using FullstackTest.Application.Common;

namespace FullstackTest.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            NotFoundException notFound => (HttpStatusCode.NotFound, notFound.Message),
            BusinessException business => (HttpStatusCode.BadRequest, business.Message),
            ArgumentException argument => (HttpStatusCode.BadRequest, argument.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = JsonSerializer.Serialize(new { message });
        await context.Response.WriteAsync(payload);
    }
}
