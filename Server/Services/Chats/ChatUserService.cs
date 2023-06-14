using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories;
using WASMChat.Data.Repositories.Chats;
using WASMChat.Server.Exceptions;

namespace WASMChat.Server.Services.Chats;

public class ChatUserService : IService
{
    private readonly ApplicationUserRepository _applicationUserRepository;
    private readonly ChatUserRepository _chatUserRepository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ChatUserService> _logger;

    public ChatUserService(
        ApplicationUserRepository applicationUserRepository, 
        ChatUserRepository chatUserRepository, 
        IMemoryCache cache, 
        ILogger<ChatUserService> logger)
    {
        _applicationUserRepository = applicationUserRepository;
        _chatUserRepository = chatUserRepository;
        _cache = cache;
        _logger = logger;
    }

    public ValueTask<ChatUser> GetOrRegisterAsync(ClaimsPrincipal principal)
    {
        var appUserId = GetAppUserId(principal);
        return GetOrRegisterAsync(appUserId);
    }
    
        
    private async ValueTask<ChatUser> GetOrRegisterAsync(string appUserId)
    {
        var cacheName = GetCacheName(appUserId);
        if (_cache.Get(cacheName) is ChatUser cachedUser)
        {
            _logger.LogInformation("Retrieved user {User} from cache", cachedUser.ApplicationUserId);
            return cachedUser;
        }
        
        var existingUser = await _chatUserRepository
            .GetByAppUserIdAsync(appUserId);

        if (existingUser is not null)
        {
            _cache.Set(cacheName, existingUser);
            _logger.LogInformation("Cached user {User}", existingUser.ApplicationUserId);
            return existingUser;
        }

        var appUser = await _applicationUserRepository.GetById(appUserId);
        NotAllowedException.ThrowIfNull(appUser);

        var newUser = new ChatUser
        {
            Name = appUser.UserName ?? Guid.NewGuid().ToString(),
            ApplicationUserId = appUserId,
        };
        appUser.ChatUser = newUser;

        await _applicationUserRepository.CommitAsync();
        _cache.Set(cacheName, newUser);
        _logger.LogInformation("Registered and cached {User}", newUser.ApplicationUserId);
        return newUser;
    }

    public ValueTask<IReadOnlyCollection<ChatUser>> GetAllUsersAsync(int page = 0)
    {
        return _chatUserRepository.GetAllAsync(page);
    }

    public async ValueTask<ChatUser> UpdateUser(ChatUser user)
    {
        var updatedUser = await _chatUserRepository.UpdateUser(user);
        return updatedUser;
    }

    private static string GetAppUserId(ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new NotAllowedException();

    private string GetCacheName(string appUserId) => $"USER_{appUserId}".ToUpper();
}