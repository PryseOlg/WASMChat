namespace WASMChat.Shared.Models.Chats;

public record ChatMessageModel
{
    public required int Id { get; init; }
    public required int ChatId { get; init; }
    public required DateTimeOffset SentTime { get; init; }
    public required ChatUserModel Author { get; init; }
    public required string Text { get; init; }
}