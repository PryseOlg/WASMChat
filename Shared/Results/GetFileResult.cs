namespace WASMChat.Shared.Results;

public record GetFileResult
{
    public required byte[] Content { get; set; }
    public required string MimeType { get; set; }
}