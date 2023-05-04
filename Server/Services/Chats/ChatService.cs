using System.Security.Claims;
using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories;
using WASMChat.Server.Extensions;
using WASMChat.Shared.Requests.Chats;

namespace WASMChat.Server.Services.Chats;

public class ChatService : IService
{
    private readonly ChatRepository _chatRepository;
    private readonly ChatUserService _chatUserService;
    private readonly ChatUserRepository _chatUserRepository;

    public ChatService(
        ChatRepository chatRepository, 
        ChatUserService chatUserService, 
        ChatUserRepository chatUserRepository)
    {
        _chatRepository = chatRepository;
        _chatUserService = chatUserService;
        _chatUserRepository = chatUserRepository;
    }

    public async ValueTask<Chat?> GetChatAsync(GetChatRequest request, ClaimsPrincipal principal)
    {
        var chat = await _chatRepository.GetChatByIdAsync(request.ChatId);
        if (chat is null) return chat;

        var user = await _chatUserService.GetOrRegister(principal);
        
        return chat.ChatUsers.Contains(user) ? chat : null;
    }

    public async ValueTask<Chat> CreateChatAsync(CreateChatRequest request, ClaimsPrincipal principal)
    {
        var user = await _chatUserService.GetOrRegister(principal);
        request.OwnerId = user.Id;

        var members = request.MemberIds
            .Append(request.OwnerId)
            .Distinct()
            .Select(async id => await _chatUserRepository.GetById(id))
            .Select(task => task.Result)
            .ExcludeNulls()
            .ToArray();
        
        var chat = new Chat
        {
            Name = request.ChatName,
            OwnerId = request.OwnerId,
            ChatUsers = members
        };

        await _chatRepository.SaveChatAsync(chat);

        return chat;
    }

    public async ValueTask<IReadOnlyCollection<Chat>> GetAllChatsAsync(GetAllChatsRequest request, ClaimsPrincipal principal)
    {
        var user = _chatUserService.GetOrRegister(principal);
        request.UserId = user.Id;

        return await _chatRepository.GetAllChats(request.UserId, request.Page);
    }
}