using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record CreateChatRequest : IRequest<CreateChatResult>
{
    public int OwnerId { get; init; }
    public required string ChatName { get; init; }
    public required int[] MemberIds { get; init; }
}