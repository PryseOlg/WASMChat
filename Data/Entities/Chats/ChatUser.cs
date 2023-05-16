using WASMChat.Data.Entities.Abstractions;

namespace WASMChat.Data.Entities.Chats;

public class ChatUser : ISoftDeletable
{
    public int Id { get; set; }
    
    public required string Name { get; set; }

    public int AvatarId { get; set; } = 1;
    public DatabaseFile? Avatar { get; set; }

    public ApplicationUser? ApplicationUser { get; set; }
    public required string ApplicationUserId { get; set; }

    public ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    
    public bool IsDeleted { get; set; }
    
    public DateTimeOffset? DeletedTime { get; set; }
}