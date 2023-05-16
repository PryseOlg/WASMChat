using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetChatRequest : IRequest<GetChatResult>
{
    public ClaimsPrincipal? User { get; init; }
    public required int ChatId { get; init; }
}