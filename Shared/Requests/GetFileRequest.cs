using MediatR;
using WASMChat.Shared.Results;

namespace WASMChat.Shared.Requests;

public record GetFileRequest : IRequest<GetFileResult>
{
    public string? Name { get; init; }
    public int? Id { get; init; }
}