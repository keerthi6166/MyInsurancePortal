using MyInsurancePortal.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // pass request down the pipeline
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error occurred.");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = context.Response.StatusCode,
                error = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found.");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = context.Response.StatusCode,
                error = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = context.Response.StatusCode,
                error = "An internal server error occurred."
            });
        }
    }
}
