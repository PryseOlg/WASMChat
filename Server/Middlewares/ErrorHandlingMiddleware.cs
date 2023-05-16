using WASMChat.Server.Exceptions;

namespace WASMChat.Server.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(
        ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = e switch
            {
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            _logger.LogInformation("Request produced exception of type {Type}, " +
                                   "returning status code {Code} and message \"{Message}\"",
                e.GetType(), context.Response.StatusCode, e.Message);
        }
    }
}