using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WASMChat.Data.Entities;
using WASMChat.Data.Entities.Abstractions;
using WASMChat.Data.Entities.Chats;

namespace WASMChat.Data;

public class ApplicationDbContext : 
    ApiAuthorizationDbContext<ApplicationUser>,
    IDataProtectionKeyContext
{
    // Таблица для чатов
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<ChatMessage> Messages => Set<ChatMessage>();
    public DbSet<ChatUser> ChatUsers => Set<ChatUser>();
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
    
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

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        var softDeletedEntries = ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(i => i.State == EntityState.Deleted);
        foreach (var softDeleted in softDeletedEntries)
        {
            softDeleted.State = EntityState.Unchanged;
            softDeleted.Property(i => i.IsDeleted).CurrentValue = true;
            softDeleted.Property(i => i.DeletedTime).CurrentValue = DateTimeOffset.UtcNow;
        }
        
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
