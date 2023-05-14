using System.Text.Json;

namespace WASMChat.Server.Middlewares;

/// <summary>
/// Adds special headers to suppress ngrok enter message.
/// </summary>
public class NgrokHeaderMiddleware : IMiddleware
{
    private const string HeaderName = "ngrok-skip-browser-warning";
    private const string HeaderValue = "true";
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Request.Headers.Add(HeaderName, HeaderValue);
        context.Response.Headers.Add(HeaderName, HeaderValue);
        await next(context);
    }
}