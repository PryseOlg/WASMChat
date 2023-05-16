using MediatR;
using WASMChat.Shared.Results;

namespace WASMChat.Shared.Requests;

public record GetFileByIdRequest : IRequest<GetFileResult>
{
    /// <example>1</example>
    public required int Id { get; init; }
}