using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public record UpdateCurrentUserResult
{
    public required ChatUserModel UpdatedUser { get; set; }
}