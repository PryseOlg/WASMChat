using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WASMChat.Server.Models;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.Repositories;



public class ChatUserRepository : RepositoryBase<ChatUser>
{
    
    //private const int ChatsPerPage = 10;
    

    public ChatUserRepository(DbContext ctx) : base(ctx)
    { }

    public ValueTask<ChatUser?> GetById(int id) => Set
        .FindAsync(id);

    public Task<ChatUser?> GetByAppUser(ApplicationUser appUser) => Set
        .FirstOrDefaultAsync(cu => cu.ApplicationUserId == appUser.Id);
    
    public Task<ChatUser?> GetByClaimsPrincipal(ClaimsPrincipal claimsPrincipal) => Set
        .FirstOrDefaultAsync(cu => cu.ApplicationUserId == claimsPrincipal
            .FindFirstValue(ClaimTypes.NameIdentifier));

    public Task<ICollection<Chat>?> GetChatsOfUser(
        int userId, int page = 0) => Set
        .Include(u => u.Chats)
        .Where(u => u.Id == userId)
        .Select(u => u.Chats)
        .FirstOrDefaultAsync();
    
    
        
    
}