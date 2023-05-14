namespace WASMChat.Server.Middlewares;

public static class DependencyInjection
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app) 
        => app.UseMiddleware<ErrorHandlingMiddleware>();

    public static IApplicationBuilder UseNgrok(this IApplicationBuilder app) 
        => app.UseMiddleware<NgrokHeaderMiddleware>();

    public static IServiceCollection AddErrorHandling(this IServiceCollection services) 
        => services.AddScoped<ErrorHandlingMiddleware>();

    public static IServiceCollection AddNgrok(this IServiceCollection services) 
        => services.AddScoped<NgrokHeaderMiddleware>();

    public static bool NgrokEnabled(this IConfiguration config)
        => config.GetValue<bool>("NgrokEnabled");
}