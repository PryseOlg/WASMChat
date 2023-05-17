using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using WASMChat.Shared.Models;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests;

public record PostFileRequest : IRequest<PostFileResult>
{
    [DefaultValue("helloworld.txt")]
    public required string FileName { get; set; }
    [DefaultValue("SGVsbG8gd29ybGQh")]
    [DataType(DataType.Upload)]
    public required string ContentBase64 { get; set; }
    [DefaultValue(System.Net.Mime.MediaTypeNames.Text.Plain)]
    public required string MimeType { get; set; }
    [DefaultValue(nameof(DatabaseFileScope.Attachment))]
    public string? Scope { get; set; }
}