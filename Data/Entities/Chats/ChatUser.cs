namespace WASMChat.Data.Entities.Chats;

public class ChatUser
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    

    public ApplicationUser? ApplicationUser { get; set; }
    public required string ApplicationUserId { get; set; }

    public ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}