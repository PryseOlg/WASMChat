using System.Security.Claims;

namespace WASMChat.Shared.Requests.Abstractions;

public interface IUserRequest
{
    public ClaimsPrincipal? User { get; set; }
    
}