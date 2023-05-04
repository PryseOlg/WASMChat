using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public class CreateChatResult
{
    public required ChatModel Chat { get; set; }
}