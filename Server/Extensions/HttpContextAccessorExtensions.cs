namespace WASMChat.Server.Extensions;

public static class HttpContextAccessorExtensions
{
    /// <summary>
    /// Safely gets <see cref="HttpContext"/> from <paramref name="httpContextAccessor"/>
    /// or throws <see cref="ArgumentNullException"/> if it is null.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static HttpContext GetContext(this IHttpContextAccessor httpContextAccessor)
    {
        HttpContext? httpContext = httpContextAccessor.HttpContext;
        ArgumentNullException.ThrowIfNull(httpContext, nameof(httpContext));
        return httpContext;
    }
}