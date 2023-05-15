namespace WASMChat.Data.Entities;

public class DatabaseFile
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required byte[] Content { get; set; }
}