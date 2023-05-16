using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetAllChatsRequest : IRequest<GetAllChatsResult>
{
    public ClaimsPrincipal? User { get; init; }
    public required int Page { get; init; } = 0;
}