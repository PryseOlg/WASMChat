using System.ComponentModel;
using MediatR;
using WASMChat.Shared.Results;

namespace WASMChat.Shared.Requests;

public record GetFileByIdRequest : IRequest<GetFileResult>
{
    [DefaultValue(1)]
    public required int Id { get; init; }
}