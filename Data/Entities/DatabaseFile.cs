using WASMChat.Shared.Models;

namespace WASMChat.Data.Entities;

public class DatabaseFile
{
    public int Id { get; set; }
    public DatabaseFileScope Scope { get; set; }
    public required string MimeType { get; set; }
    public required string Name { get; set; }
    public required byte[] Content { get; set; }
}