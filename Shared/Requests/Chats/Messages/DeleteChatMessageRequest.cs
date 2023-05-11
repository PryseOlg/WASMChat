using MediatR;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Shared.Requests.Chats.Messages;

public class DeleteChatMessageRequest : IRequest<DeleteChatMessageResult>
{
    public int AuthorId { get; init; }
    public required int MessageId { get; init; }
}