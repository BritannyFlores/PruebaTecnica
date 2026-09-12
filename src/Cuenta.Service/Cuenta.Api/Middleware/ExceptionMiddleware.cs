using System.Net;
using System.Text.Json;
using Cuenta.Application.Exceptions;

namespace Cuenta.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error no controlado");
            await ManejarExcepcionAsync(context, ex);
        }
    }

    private static Task ManejarExcepcionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, mensaje) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message),
            BusinessRuleException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocurrió un error interno en el servidor")
        };

        context.Response.StatusCode = (int)statusCode;

        var respuesta = new
        {
            error = mensaje,
            codigo = (int)statusCode
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(respuesta));
    }
}