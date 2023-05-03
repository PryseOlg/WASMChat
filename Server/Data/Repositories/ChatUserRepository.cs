using Microsoft.EntityFrameworkCore;
using WASMChat.Server.Models;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.Repositories;

public class ChatUserRepository : RepositoryBase<ChatUser>
{
    public ChatUserRepository(DbContext ctx) : base(ctx)
    { }

    public ValueTask<ChatUser?> GetById(int id) => Set
        .FindAsync(id);

    public Task<ChatUser?> GetByAppUser(ApplicationUser appUser) => Set
        .FirstOrDefaultAsync(cu => cu.ApplicationUserId == appUser.Id);
}