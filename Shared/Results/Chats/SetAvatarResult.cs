using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public record SetAvatarResult
{
    public required ChatUserModel UpdatedUser { get; set; }
}