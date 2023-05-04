using Microsoft.AspNetCore.Identity;
using WASMChat.Data.Entities.Chats;

namespace WASMChat.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public ChatUser ChatUser { get; set; }
}
