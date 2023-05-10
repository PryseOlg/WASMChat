using System.Security.Claims;
using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories;
using WASMChat.Server.Exceptions;
using WASMChat.Shared.Requests.Chats;

namespace WASMChat.Server.Services.Chats;

public class ChatUserService : IService
{
    private readonly ApplicationUserRepository _applicationUserRepository;
    private readonly ChatUserRepository _chatUserRepository;

    public ChatUserService(
        ApplicationUserRepository applicationUserRepository, 
        ChatUserRepository chatUserRepository)
    {
        _applicationUserRepository = applicationUserRepository;
        _chatUserRepository = chatUserRepository;
    }

    public ValueTask<ChatUser> GetOrRegisterAsync(ClaimsPrincipal principal)
    {
        var appUserId = GetAppUserId(principal);
        return GetOrRegisterAsync(appUserId);
    }
    
        
    private async ValueTask<ChatUser> GetOrRegisterAsync(string appUserId)
    {
        var existingUser = await _chatUserRepository
            .GetByAppUserIdAsync(appUserId);

        if (existingUser is not null) return existingUser;

        var appUser = await _applicationUserRepository.GetById(appUserId);
        UnauthorizedException.ThrowIfNull(appUser);

        var newUser = new ChatUser
        {
            Name = appUser.UserName ?? Guid.NewGuid().ToString(),
            ApplicationUserId = appUserId,
        };
        appUser.ChatUser = newUser;

        await _applicationUserRepository.CommitAsync();
        return newUser;
    }

    public ValueTask<IReadOnlyCollection<ChatUser>> GetAllUsersAsync(int page = 0)
    {
        return _chatUserRepository.GetAllAsync(page);
    }

    private static string GetAppUserId(ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedException();
}