using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public record GetChatUserResult
{
    public required ChatUserModel User { get; init; }
}