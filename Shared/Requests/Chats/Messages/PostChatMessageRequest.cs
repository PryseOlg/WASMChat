using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Shared.Requests.Chats.Messages;

public record PostChatMessageRequest : IRequest<PostChatMessageResult>
{
    public const int MaxTextLength = 2000;
    
    public int AuthorId { get; init; }
    public int ChatId { get; init; }
    public ClaimsPrincipal? User { get; init; }
    [MinLength(1), MaxLength(MaxTextLength)]
    public required string Text { get; init; }
}