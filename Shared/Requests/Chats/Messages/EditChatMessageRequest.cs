using MediatR;
using WASMChat.Shared.Models.Chats;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Shared.Requests.Chats.Messages;

public record EditChatMessageRequest : IRequest<EditChatMessageResult>
{
    public required ChatMessageModel EditedMessage { get; init; }
}