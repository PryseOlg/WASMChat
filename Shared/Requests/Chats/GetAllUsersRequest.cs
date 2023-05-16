using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetAllUsersRequest : IRequest<GetAllUsersResult>
{
    public required int Page { get; init; } = 0;
}