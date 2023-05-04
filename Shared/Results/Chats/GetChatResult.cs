using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public class GetChatResult
{
    public required ChatModel Chat { get; set; }
}