using Microsoft.EntityFrameworkCore;

namespace WASMChat.Data.Repositories;

public abstract class RepositoryBase<T>
    where T: class
{
    /// <summary>
    /// The encapsulated <see cref="DbContext"/>.
    /// </summary>
    private readonly DbContext _ctx;

    protected RepositoryBase(DbContext ctx)
    {
        _ctx = ctx;
    }
    
    /// <summary>
    /// The accessible <see cref="DbSet{TEntity}"/>, all the
    /// internal operations are expected to work with it.
    /// </summary>
    protected DbSet<T> Set => _ctx.Set<T>();
    
    /// <summary>
    /// Saves all changes made in this <see cref="RepositoryBase{T}"/>.
    /// </summary>
    /// <returns></returns>
    public Task CommitAsync() => _ctx.SaveChangesAsync();
}