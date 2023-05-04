namespace WASMChat.Shared.Requests.Chats;

public class GetAllChatsRequest
{
    public int UserId { get; set; }
    public int Page { get; set; } = 0;
}