using System.Web;
using WASMChat.Data.Entities;
using WASMChat.Data.Repositories;
using WASMChat.Server.Exceptions;
using WASMChat.Shared.Models;

namespace WASMChat.Server.Services;

public class DatabaseFileService : IService
{
    private readonly DatabaseFileRepository _repo;

    public DatabaseFileService(
        DatabaseFileRepository repo)
    {
        _repo = repo;
    }

    public async ValueTask<DatabaseFile> GetFileByNameAsync(string name)
    {
        DatabaseFile? file = await _repo.GetByNameAsync(name);
        NotFoundException.ThrowIfNull(file);
        return file;
    }
    
    public async ValueTask<DatabaseFile> GetFileByIdAsync(int id)
    {
        DatabaseFile? file = await _repo.GetByIdAsync(id);
        NotFoundException.ThrowIfNull(file);
        return file;
    }

    public async ValueTask<DatabaseFile> SaveDatabaseFile(
        byte[] content, string mimeType, string fileName, string? scope = null)
    {
        var newFileName = Path.GetRandomFileName() + Path.GetExtension(fileName);

        DatabaseFile savedFile = new()
        {
            Content = content,
            MimeType = mimeType,
            Name = newFileName
        };

        if (Enum.TryParse(scope, out DatabaseFileScope parsedScope))
        {
            savedFile.Scope = parsedScope;
        }

        await _repo.SaveFileAsync(savedFile);

        return savedFile;
    }
}