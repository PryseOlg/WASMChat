using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats.Messages;

public record EditChatMessageResult
{
    public required ChatMessageModel EditedMessage { get; init; }
}