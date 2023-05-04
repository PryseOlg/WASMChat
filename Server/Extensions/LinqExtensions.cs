namespace WASMChat.Server.Extensions;

public static class LinqExtensions
{
    public static IEnumerable<T> ExcludeNulls<T>(this IEnumerable<T?> collection) => collection
        .Where(t => t is not null)
        .Cast<T>();
}