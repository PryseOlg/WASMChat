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
    
    [DefaultValue(new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 })]
    [DataType(DataType.Upload)]
    public required byte[] Content { get; set; }
    
    [DefaultValue(System.Net.Mime.MediaTypeNames.Text.Plain)]
    public required string MimeType { get; set; }
    
    [DefaultValue(nameof(DatabaseFileScope.Attachment))]
    public string? Scope { get; set; }
}