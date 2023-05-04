using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WASMChat.Data.Entities;

namespace WASMChat.Data.Repositories;

public class ApplicationUserRepository : RepositoryBase<ApplicationUser>
{
    public ApplicationUserRepository(DbContext ctx) : base(ctx)
    { }

    public ValueTask<ApplicationUser?> GetById(string id) => Set
        .FindAsync(id);
}