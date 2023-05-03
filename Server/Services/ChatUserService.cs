using System.Security.Authentication;
using WASMChat.Server.Data.Repositories;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Services;

public class ChatUserService
{
    private readonly ApplicationUserRepository _applicationUserRepository;
    private readonly ChatUserRepository _chatUserRepository;

    public ChatUserService(ApplicationUserRepository applicationUserRepository, ChatUserRepository chatUserRepository)
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

        var newUser = new ChatUser()
        {
            Name = appUser.UserName ?? Guid.NewGuid().ToString()
        };
        appUser.ChatUser = newUser;

        await _applicationUserRepository.CommitAsync();
        return newUser;
    }
}