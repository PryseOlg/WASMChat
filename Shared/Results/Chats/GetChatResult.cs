using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public record GetChatResult
{
    public required ChatUserModel CurrentUser { get; init; }
    public required ChatModel Chat { get; init; }
}