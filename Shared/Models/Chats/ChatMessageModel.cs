namespace WASMChat.Shared.Models.Chats;

public class ChatMessageModel
{
    public required int ChatId { get; set; }
    public required DateTimeOffset SentTime { get; set; }
    public required ChatUserModel Author { get; set; }
    public required string Text { get; set; }
}