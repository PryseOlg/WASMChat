using System.Diagnostics.CodeAnalysis;

namespace WASMChat.Server.Exceptions;

public class UnauthorizedException : Exception
{
    private const string DefaultMessage = "You are not authorized to do this!";
    
    public UnauthorizedException(string? message = null) : base(message ?? DefaultMessage)
    {
    }
    
    public static void ThrowIfNull([NotNull] object? param, string? message = null)
    {
        if (param is null)
        {
            throw new UnauthorizedException(message);
        }
    }
}