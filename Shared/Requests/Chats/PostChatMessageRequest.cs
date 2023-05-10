using System.ComponentModel.DataAnnotations;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record PostChatMessageRequest : IRequest<PostChatMessageResult>
{
    public int AuthorId { get; set; }
    public int ChatId { get; set; }
    [MaxLength(2000)]
    public required string Text { get; set; }
}