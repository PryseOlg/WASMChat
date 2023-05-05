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

    public async ValueTask<ChatMessage> SendMessageAsync(PostChatMessageRequest request, HttpContext ctx)
    {
        var user = await _chatUserService.GetOrRegisterAsync(ctx.User);
        request.AuthorId = user.Id;
        
        ChatMessage message = new()
        {
            MessageText = request.Text,
            AuthorId = request.AuthorId,
            ChatId = request.ChatId,
        };

        await _chatMessageRepository.SaveMessageAsync(message);
        
        return message;
    }

    public ValueTask<IReadOnlyCollection<ChatMessage>> FetchMessages(int chatId, int page = 0) =>
        _chatMessageRepository.GetMessages(chatId, page);
}