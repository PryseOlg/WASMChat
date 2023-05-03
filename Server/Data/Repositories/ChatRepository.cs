using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.Repositories;

public class ChatRepository : RepositoryBase<Chat>
{
    public ChatRepository(DbContext ctx) : base(ctx)
    { }

    public Task<Chat?> GetChatByIdAsync(int id) => Set
        .Include(x => x.ChatUsers)
        .Include(x => x.Messages)
        .ThenInclude(x => x.Author)
        .FirstOrDefaultAsync(x => x.Id == id);

    public async ValueTask<Chat> SaveChat(Chat chat)
    {
        Set.Add(chat);
        await CommitAsync();
        return chat;
    }

}