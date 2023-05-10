using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record CreateChatRequest : IRequest<CreateChatResult>
{
    public int OwnerId { get; set; }
    public required string ChatName { get; set; }
    public required int[] MemberIds { get; set; }
}