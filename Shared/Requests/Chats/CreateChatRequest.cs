namespace WASMChat.Shared.Requests.Chats;

public class CreateChatRequest
{
    public int OwnerId { get; set; }
    public required string ChatName { get; set; }
    public required int[] MemberIds { get; set; }
}