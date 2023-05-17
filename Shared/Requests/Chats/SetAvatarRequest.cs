using System.ComponentModel;
using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Requests.Abstractions;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record SetAvatarRequest : 
    IRequest<SetAvatarResult>,
    IUserRequest
{
    public ClaimsPrincipal? User { get; set; }
    [DefaultValue(1)] 
    public required int FileId { get; init; } = 1;
}