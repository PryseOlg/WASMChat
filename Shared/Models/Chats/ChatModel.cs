namespace WASMChat.Shared.Models.Chats;

public class ChatModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required ChatUserModel[] Users { get; set; }
    public required ChatMessageModel[] Messages { get; set; }
}