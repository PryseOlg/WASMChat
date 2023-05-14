using WASMChat.Server.Exceptions;

namespace WASMChat.Server.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (UnauthorizedAccessException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (ArgumentNullException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        catch (NotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}