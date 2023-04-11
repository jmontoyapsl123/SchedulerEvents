using System.Net;

namespace SchedulerEventApi.Extensions;
public class ExceptionHandlerExtension
{

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerExtension> _logger;

    public ExceptionHandlerExtension(RequestDelegate next, ILogger<ExceptionHandlerExtension> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync($"Internal Server Error: {exception.Message}");
    }
}