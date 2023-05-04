using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public class PostChatMessageResult
{
    public required ChatMessageModel Message { get; set; }
}