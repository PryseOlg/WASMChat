using System.Runtime.Serialization;

namespace WASMChat.Shared.Requests.Chats;

public class GetChatRequest
{
    public int UserId { get; set; }
    public required int ChatId { get; set; }
}