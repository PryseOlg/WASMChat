using System.ComponentModel;
using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Requests.Abstractions;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record UpdateCurrentUserRequest : 
    IRequest<UpdateCurrentUserResult>,
    IUserRequest
{
    public ClaimsPrincipal? User { get; set; }
    [DefaultValue(1)] 
    public required int AvatarId { get; init; } = 1;
    
    public required string UserName { get; init; }
}