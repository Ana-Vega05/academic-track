using System.Net;
using System.Text.Json;

namespace AcademicTrack.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ArgumentException ex)
        {
            await EscribirProblema(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            // ej: "el indicador X no existe" — dato inconsistente, no un bug del servidor
            await EscribirProblema(context, HttpStatusCode.UnprocessableEntity, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado en {Path}", context.Request.Path);
            await EscribirProblema(context, HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
        }
    }

    private static Task EscribirProblema(HttpContext context, HttpStatusCode statusCode, string detail)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;
        var problema = new { status = (int)statusCode, detail };
        return context.Response.WriteAsync(JsonSerializer.Serialize(problema));
    }
}