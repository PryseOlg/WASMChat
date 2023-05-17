using System.Diagnostics.CodeAnalysis;

namespace WASMChat.Server.Exceptions;

public class NotAllowedException : Exception
{
    private const string DefaultMessage = "You are not authorized to do this!";
    
    public NotAllowedException(string? message = null) : base(message ?? DefaultMessage)
    {
    }
    
    public static void ThrowIfNull([NotNull] object? param, string? message = null)
    {
        if (param is null) throw new NotAllowedException(message);
    }

    public static void ThrowIf(bool check, string? message = null)
    {
        if (check) throw new NotAllowedException(message);
    }
}