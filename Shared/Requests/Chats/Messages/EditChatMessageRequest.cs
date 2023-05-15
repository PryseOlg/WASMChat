using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Shared.Requests.Chats.Messages;

public record EditChatMessageRequest : IRequest<EditChatMessageResult>
{
    public ClaimsPrincipal? User { get; init; } 
    public int MessageId { get; init; }
    public int? ReferencedMessageId { get; init; }
    
    [MinLength(Constants.Messages.MinMessageTextLength)]
    [MaxLength(Constants.Messages.MaxMessageTextLength)]
    public string? NewText { get; init; }
}