using System.ComponentModel.DataAnnotations;

namespace WASMChat.Shared.Requests.Chats;

public class PostChatMessageRequest
{
    public int AuthorId { get; set; }
    public int ChatId { get; set; }
    [MaxLength(2000)]
    public required string Text { get; set; }
}