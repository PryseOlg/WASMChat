namespace WASMChat.Shared.Messages;

public class ChatUser
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public string ApplicationUserId { get; set; }

    public ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();
    
    



}