using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WASMChat.Server.Models;

namespace WASMChat.Server.Data.Repositories;

public class ApplicationUserRepository : RepositoryBase<ApplicationUser>
{
    public ApplicationUserRepository(DbContext ctx) : base(ctx)
    { }

    public ValueTask<ApplicationUser?> GetById(string id) => Set
        .FindAsync(id);

    public ValueTask<ApplicationUser?> GetByClaimsPrincipal(ClaimsPrincipal principal) => Set
        .FindAsync(principal.FindFirstValue(ClaimTypes.NameIdentifier));
}