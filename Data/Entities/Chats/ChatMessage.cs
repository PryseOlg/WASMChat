using System.ComponentModel.DataAnnotations;
using WASMChat.Data.Entities.Abstractions;
using WASMChat.Data.Entities.Files;

namespace WASMChat.Data.Entities.Chats;

public class ChatMessage : ISoftDeletable
{
    public int Id { get; set; }
    
    public DateTimeOffset DateTimeSent { get; set; }
    [MaxLength(2000)]
    public required string MessageText { get; set; }
    
    public int? AttachmentId { get; set; }
    public DatabaseFile? Attachment { get; set; }
    
    public int? ReferencedMessageId { get; set; }
    public ChatMessage? ReferencedMessage { get; set; }
    
    public ChatUser? Author { get; set; }
    public int AuthorId { get; set; }
    
    public Chat? Chat { get; set; }
    public int ChatId { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTimeOffset? DeletedTime { get; set; }
}