using Microsoft.AspNetCore.Identity;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Models;

public class ApplicationUser : IdentityUser
{
    public ChatUser ChatUser { get; set; }
}
