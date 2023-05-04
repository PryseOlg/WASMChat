using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public class GetAllChatsResult
{
    public required ChatModel[] Chats { get; set; }
}