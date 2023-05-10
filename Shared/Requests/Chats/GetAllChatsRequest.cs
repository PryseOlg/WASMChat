using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetAllChatsRequest : IRequest<GetAllChatsResult>
{
    public int UserId { get; init; }
    public int Page { get; init; } = 0;
}