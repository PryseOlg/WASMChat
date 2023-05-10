using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public record GetAllUsersResult
{
    public required ChatUserModel[] Users { get; init; }
}