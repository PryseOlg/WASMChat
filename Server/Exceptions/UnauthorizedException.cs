namespace WASMChat.Server.Exceptions;

public class UnauthorizedException : Exception
{
    private const string DefaultMessage = "You are not authorized to do this!";
    
    public UnauthorizedException(string? message = null) : base(message ?? DefaultMessage)
    {
    }
}