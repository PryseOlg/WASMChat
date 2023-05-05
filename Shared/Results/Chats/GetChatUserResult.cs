using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public class GetChatUserResult
{
    public required ChatUserModel User { get; set; }
}