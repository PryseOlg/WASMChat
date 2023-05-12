using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Shared.Requests.Chats.Messages;

public record DeleteChatMessageRequest : IRequest<DeleteChatMessageResult>
{
    public ClaimsPrincipal? User { get; init; }
    public int AuthorId { get; init; }
    
    public required int ChatId { get; init; }
    public required int MessageId { get; init; }
}