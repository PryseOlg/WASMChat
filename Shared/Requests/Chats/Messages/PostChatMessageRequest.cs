using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Shared.Requests.Chats.Messages;

public record PostChatMessageRequest : IRequest<PostChatMessageResult>
{
    public ClaimsPrincipal? User { get; init; }
    public int AuthorId { get; init; }
    public int? ReferencedMessageId { get; init; }
    public int? AttachmentId { get; init; }
    public required int ChatId { get; init; }
    [MinLength(Constants.Messages.MinMessageTextLength)]
    [MaxLength(Constants.Messages.MaxMessageTextLength)]
    public required string Text { get; init; }
    
    
}