using Microsoft.EntityFrameworkCore;
using WASMChat.Data.Entities.Chats;

namespace WASMChat.Data.Repositories;



public class ChatUserRepository : RepositoryBase<ChatUser>
{

    //private const int ChatsPerPage = 10;
    private readonly ApplicationUserRepository _appUserRepo;

    public ChatUserRepository(DbContext ctx, ApplicationUserRepository appUserRepo) : base(ctx)
    {
        _appUserRepo = appUserRepo;
    }

    public ValueTask<ChatUser?> GetById(int id) => Set
        .FindAsync(id);

    public Task<ChatUser?> GetByAppUserId(string appUserId) => Set
        .FirstOrDefaultAsync(x => x.ApplicationUserId == appUserId);
}