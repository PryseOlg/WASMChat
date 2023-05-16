using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetCurrentChatUserRequest : IRequest<GetCurrentChatUserResult>
{
    public ClaimsPrincipal User { get; init; } = null!;
}