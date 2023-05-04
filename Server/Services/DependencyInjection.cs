namespace WASMChat.Server.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        foreach (var service in GetServiceTypes())
        {
            services.AddScoped(service);
        }

        return services;
    }

    private static IEnumerable<Type> GetServiceTypes() => 
        typeof(DependencyInjection).Assembly
        .GetTypes()
        .Where(IsServiceType);

    private static bool IsServiceType(Type type) =>
        type.IsAssignableTo(typeof(IService)) &&
        type.IsAbstract is false;
}