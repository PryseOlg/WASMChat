using System.Security.Authentication;
using System.Security.Claims;
using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories;
using WASMChat.Server.Exceptions;

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
    
    public async Task<ChatUser> GetOrRegister(string appUserId)
    {
        var existingUser = await _chatUserRepository
            .GetByAppUserId(appUserId);

        if (existingUser is not null) return existingUser;

        var appUser = await _applicationUserRepository.GetById(appUserId);
        if (appUser is null) throw new AuthenticationException();

        var newUser = new ChatUser
        {
            Name = appUser.UserName ?? Guid.NewGuid().ToString(),
            ApplicationUserId = appUserId,
        };
        appUser.ChatUser = newUser;

        await _applicationUserRepository.CommitAsync();
        return newUser;
    }

    public Task<ChatUser> GetOrRegister(ClaimsPrincipal principal)
    {
        var appUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedException();
        return GetOrRegister(appUserId);
    }
}