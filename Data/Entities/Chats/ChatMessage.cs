using System.ComponentModel.DataAnnotations;

namespace WASMChat.Data.Entities.Chats;

public class ChatMessage
{
    public int Id { get; set; }
    
    public DateTimeOffset DateTimeSent { get; set; }
    [MaxLength(2000)]
    public required string MessageText { get; set; }
    
    public ChatUser? Author { get; set; }
    public int AuthorId { get; set; }
    
    public Chat? Chat { get; set; }
    public int ChatId { get; set; }
}