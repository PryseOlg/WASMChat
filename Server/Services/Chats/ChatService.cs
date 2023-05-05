using System.Security.Authentication;
using System.Security.Claims;
using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories;
using WASMChat.Server.Exceptions;
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

    public async ValueTask<Chat> GetChatAsync(GetChatRequest request, HttpContext ctx)
    {
        var chat = await _chatRepository.GetChatByIdAsync(request.ChatId);
        NotFoundException.ThrowIfNull(chat, "Чат не найден");

        var user = await _chatUserService.GetOrRegisterAsync(ctx.User);
        
        return chat.ChatUsers.Contains(user) ? 
            chat : 
            throw new AuthenticationException("У вас нет прав на просмотр этого чата");
    }

    public async ValueTask<Chat> CreateChatAsync(CreateChatRequest request, HttpContext ctx)
    {
        var user = await _chatUserService.GetOrRegisterAsync(ctx.User);
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

    public async ValueTask<IReadOnlyCollection<Chat>> GetAllChatsAsync(GetAllChatsRequest request, HttpContext ctx)
    {
        var user = await _chatUserService.GetOrRegisterAsync(ctx.User);
        request.UserId = user.Id;

        return await _chatRepository.GetAllChats(request.UserId, request.Page);
    }
}