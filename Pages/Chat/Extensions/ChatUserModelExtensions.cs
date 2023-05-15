using Microsoft.AspNetCore.Components;
using WASMChat.Shared.Models.Chats;

namespace WASMChat.Pages.Chat.Extensions;

public static class ChatUserModelExtensions
{
    public static MarkupString FormatUserName(this ChatUserModel user)
        => (MarkupString)$"{user.UserName}<b>#{user.Id}</b>";
}