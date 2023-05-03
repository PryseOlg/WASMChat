using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WASMChat.Client;
using WASMChat.Server.Models;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.Repositories;



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

    public Task<ICollection<Chat>?> GetChatsOfUser(
        int userId, int page = 0) => Set
        .Include(u => u.Chats)
        .Where(u => u.Id == userId)
        .Select(u => u.Chats)
        .FirstOrDefaultAsync();

    public Task<ChatUser?> GetByAppUserId(string appUserId) => Set
        .FirstOrDefaultAsync(x => x.ApplicationUserId == appUserId);

    public async Task Update(ChatUser chatUser)
    {
        Set.Update(chatUser);
        await CommitAsync();
    }
}