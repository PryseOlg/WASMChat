namespace WASMChat.Server.Mappers;

public static class DependencyInjection
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        foreach (var mapper in GetMapperTypes())
        {
            services.AddScoped(mapper);
        }

        return services;
    }

    private static IEnumerable<Type> GetMapperTypes() => 
        typeof(DependencyInjection).Assembly
        .GetTypes()
        .Where(IsMapperType);

    private static bool IsMapperType(Type type) =>
        type.IsAssignableTo(typeof(IMapper)) &&
        type.IsAbstract is false;
}