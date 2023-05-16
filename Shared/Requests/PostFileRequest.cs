using System.ComponentModel.DataAnnotations;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests;

public record PostFileRequest : IRequest<PostFileResult>
{
    /// <example>Pepega.txt</example>
    public required string FileName { get; set; }
    /// <example>SGVsbG8gd29ybGQh</example>
    [DataType(DataType.Upload)]
    public required string ContentBase64 { get; set; }
    /// <example>text/plain</example>
    public required string MimeType { get; set; }
    /// <example>Attachment</example>
    public string? Scope { get; set; }
}