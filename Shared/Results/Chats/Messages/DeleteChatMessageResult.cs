namespace WASMChat.Shared.Results.Chats.Messages;

public record DeleteChatMessageResult
{
    public required int MessageId { get; init; }
}