using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public class GetAllUsersResult
{
    public required ChatUserModel[] Users { get; set; }
}