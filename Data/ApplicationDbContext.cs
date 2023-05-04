using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WASMChat.Data.Entities;
using WASMChat.Data.Entities.Chats;

namespace WASMChat.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    // Таблица для чатов
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<ChatMessage> Messages => Set<ChatMessage>();
    public DbSet<ChatUser> ChatUsers => Set<ChatUser>();

    private static bool _firstRun = true;
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
        if (_firstRun)
        {
            Database.Migrate();
            _firstRun = false;
        }
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
