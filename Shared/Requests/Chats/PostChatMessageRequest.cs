using System.ComponentModel.DataAnnotations;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record PostChatMessageRequest : IRequest<PostChatMessageResult>
{
    public const int MaxTextLength = 2000;
    
    public int AuthorId { get; init; }
    public int ChatId { get; init; }
    [MinLength(1), MaxLength(MaxTextLength)]
    public required string Text { get; init; }
}