using System.ComponentModel;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetAllUsersRequest : IRequest<GetAllUsersResult>
{
    [DefaultValue(0)]
    public required int Page { get; init; }
}