using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Requests.Abstractions;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetCurrentChatUserRequest : 
    IRequest<GetCurrentChatUserResult>,
    IUserRequest
{
    public ClaimsPrincipal? User { get; set; }
}