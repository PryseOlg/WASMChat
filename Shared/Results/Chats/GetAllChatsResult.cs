using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public record GetAllChatsResult
{
    public required ChatModel[] Chats { get; init; }
}