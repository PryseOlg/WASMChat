namespace WASMChat.Shared.Messages;

public class Chat
{
    public int Id { get; set; }
    
    public required string Name { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
    
}