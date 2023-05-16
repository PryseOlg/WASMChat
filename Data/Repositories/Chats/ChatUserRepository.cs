using Microsoft.EntityFrameworkCore;
using WASMChat.Data.Entities.Chats;

namespace WASMChat.Data.Repositories.Chats;



public class ChatUserRepository : RepositoryBase<ChatUser>
{
    public ChatUserRepository(DbContext ctx) : base(ctx)
    { }

    private const int UsersPerPage = 50;

    public ValueTask<ChatUser?> GetByIdAsync(int id) => Set
        .FindAsync(id);

    public async ValueTask<ChatUser?> GetByAppUserIdAsync(string appUserId) => await Set
        .FirstOrDefaultAsync(x => x.ApplicationUserId == appUserId);

    public async ValueTask<IReadOnlyCollection<ChatUser>> GetAllAsync(int page = 0) => await Set
        .OrderBy(u => u.Id)
        .Skip(page * UsersPerPage)
        .Take(UsersPerPage)
        .ToArrayAsync();
}