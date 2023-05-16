using Microsoft.EntityFrameworkCore;
using WASMChat.Data.Entities.Files;

namespace WASMChat.Data.Repositories;

public class DatabaseFileRepository : RepositoryBase<DatabaseFile>
{
    public DatabaseFileRepository(DbContext ctx) : base(ctx)
    { }

    public async ValueTask<DatabaseFile?> GetByNameAsync(string name)
        => await Set.FirstOrDefaultAsync(df => df.Name == name);

    public async ValueTask<DatabaseFile?> GetByIdAsync(int id)
        => await Set.FindAsync(id);

    public async ValueTask<DatabaseFile> SaveFile(DatabaseFile file)
    {
        Set.Update(file);
        await CommitAsync();
        return file;
    }
}