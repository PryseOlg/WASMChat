using MediatR;
using WASMChat.Shared.Results;

namespace WASMChat.Shared.Requests;

public record GetFileByNameRequest : IRequest<GetFileResult>
{
    /// <example>DefaultAvatar.jpg</example>
    public string? Name { get; init; }
}