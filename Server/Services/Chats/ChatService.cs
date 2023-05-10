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

    public async ValueTask<Chat> GetChatAsync(int chatId, int chatUserId)
    {
        var chat = await _chatRepository.GetChatByIdAsync(chatId);
        NotFoundException.ThrowIfNull(chat, "Чат не найден");
        
        return chat.ChatUsers.Any(u => u.Id == chatUserId) ? 
            chat : 
            throw new AuthenticationException("У вас нет прав на просмотр этого чата");
    }

    public async ValueTask<Chat> CreateChatAsync(string chatName, int ownerId, IEnumerable<int> memberIds)
    {
        var members = memberIds
            .Append(ownerId)
            .Distinct()
            .Select(async id => await _chatUserRepository.GetByIdAsync(id))
            .Select(task => task.Result)
            .ExcludeNulls()
            .ToArray();
        
        var chat = new Chat
        {
            Name = chatName,
            OwnerId = ownerId,
            ChatUsers = members
        };

        await _chatRepository.SaveChatAsync(chat);

        return chat;
    }

    public ValueTask<IReadOnlyCollection<Chat>> GetAllChatsAsync(int userId, int page = 0)
        => _chatRepository.GetAllChats(userId, page);
}