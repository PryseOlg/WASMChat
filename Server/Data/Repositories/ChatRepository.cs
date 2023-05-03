using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.Repositories;

public class ChatRepository : RepositoryBase<Chat>
{
    public ChatRepository(DbContext ctx) : base(ctx)
    { }

    public ValueTask<Chat?> GetChatByIdAsync(int id) 
        => Set.FindAsync(id);
    
    public async ValueTask<Chat> SaveChat(Chat chat)
    {
        Set.Add(chat);
        await CommitAsync();
        return chat;
    }
}