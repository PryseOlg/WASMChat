using System.Security.Claims;
using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories;
using WASMChat.Shared.Requests.Chats;

namespace WASMChat.Server.Services.Chats;

public class ChatMessageService : IService
{
    private readonly ChatMessageRepository _chatMessageRepository;
    private readonly ChatUserService _chatUserService;

    public ChatMessageService(
        ChatMessageRepository chatMessageRepository, 
        ChatUserService chatUserService)
    {
        _chatMessageRepository = chatMessageRepository;
        _chatUserService = chatUserService;
    }

    public async ValueTask<ChatMessage> SendMessageAsync(string text, int authorId, int chatId)
    {
        ChatMessage message = new()
        {
            MessageText = text,
            AuthorId = authorId,
            ChatId = chatId,
        };

        await _chatMessageRepository.SaveMessageAsync(message);
        
        return message;
    }

    public ValueTask<IReadOnlyCollection<ChatMessage>> FetchMessagesAsync(int chatId, int page = 0) =>
        _chatMessageRepository.GetMessagesAsync(chatId, page);
}