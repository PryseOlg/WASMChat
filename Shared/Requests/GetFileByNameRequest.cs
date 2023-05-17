using System.ComponentModel;
using MediatR;
using WASMChat.Shared.Results;

namespace WASMChat.Shared.Requests;

public record GetFileByNameRequest : IRequest<GetFileResult>
{
    [DefaultValue("DefaultAvatar.jpg")]
    public required string Name { get; init; }
}