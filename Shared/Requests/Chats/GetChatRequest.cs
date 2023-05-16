using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetChatRequest : IRequest<GetChatResult>
{
    public int UserId { get; init; }
    public required int ChatId { get; init; }
}