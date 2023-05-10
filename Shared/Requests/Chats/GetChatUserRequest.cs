using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetChatUserRequest : IRequest<GetChatUserResult>
{
    public string? AppUserId { get; init; }
}