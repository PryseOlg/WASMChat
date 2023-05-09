using WASMChat.Data.Entities.Abstractions;

namespace WASMChat.Data.Entities.Chats;

public class Chat : ISoftDeletable
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public ChatUser? Owner { get; set; }
    public int OwnerId { get; set; }

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    
    public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();

    public bool IsDeleted { get; set; }
    
    public DateTimeOffset? DeletedTime { get; set; }
}