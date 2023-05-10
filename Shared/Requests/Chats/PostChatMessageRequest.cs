using System.ComponentModel.DataAnnotations;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record PostChatMessageRequest : IRequest<PostChatMessageResult>
{
    public int AuthorId { get; init; }
    public int ChatId { get; init; }
    [MaxLength(2000)]
    public required string Text { get; init; }
}