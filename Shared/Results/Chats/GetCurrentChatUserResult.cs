using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public record GetCurrentChatUserResult
{
    public required ChatUserModel User { get; init; }
}