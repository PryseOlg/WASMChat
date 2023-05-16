namespace WASMChat.Shared.Results.Chats;

public record PostFileResult
{
    public required int FileId { get; set; }
    public required string FileName { get; set; }
}