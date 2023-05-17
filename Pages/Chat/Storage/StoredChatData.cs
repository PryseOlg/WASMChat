using WASMChat.Shared.Models.Chats;

namespace WASMChat.Pages.Chat.Storage;

public record StoredChatData
{
    public required ChatModel Chat { get; init; }
    public required bool IsPreviewLoaded { get; init; }
}