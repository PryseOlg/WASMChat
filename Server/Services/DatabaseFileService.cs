using WASMChat.Data.Entities.Files;
using WASMChat.Data.Repositories;
using WASMChat.Server.Exceptions;

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
}