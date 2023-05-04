namespace WASMChat.Server.Validators;

public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        foreach (var service in GetValidatorTypes())
        {
            services.AddScoped(service);
        }

        return services;
    }

    private static IEnumerable<Type> GetValidatorTypes() => 
        typeof(DependencyInjection).Assembly
        .GetTypes()
        .Where(IsValidatorType);

    private static bool IsValidatorType(Type type) =>
        type.IsAssignableTo(typeof(IValidator)) &&
        type.IsAbstract is false;
}