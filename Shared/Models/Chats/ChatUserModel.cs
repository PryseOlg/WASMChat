namespace WASMChat.Shared.Models.Chats;

public record ChatUserModel
{
    public required int Id { get; init; }
    public required int AvatarId { get; init; }
    public required string UserName { get; init; }
}