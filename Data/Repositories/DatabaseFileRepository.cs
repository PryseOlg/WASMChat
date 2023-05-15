using Microsoft.EntityFrameworkCore;
using WASMChat.Data.Entities;

namespace WASMChat.Data.Repositories;

public class DatabaseFileRepository : RepositoryBase<DatabaseFile>
{
    public DatabaseFileRepository(DbContext ctx) : base(ctx)
    { }

    public async ValueTask<DatabaseFile?> GetByNameAsync(string name)
        => await Set.FirstOrDefaultAsync(df => df.Name == name);
}