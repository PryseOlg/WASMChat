namespace WASMChat.Data.Entities.Abstractions;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
}