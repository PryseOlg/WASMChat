using System.Security.Claims;
using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories;
using WASMChat.Server.Exceptions;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;

namespace WASMChat.Server.Mappers.Chats;

public class ChatMessageMapper : IMapper
{
    private readonly ChatUserService _chatUserService;
    private readonly ChatRepository _chatRepository;

    public ChatMessageMapper(
        ChatUserService chatUserService, 
        ChatRepository chatRepository)
    {
        _chatUserService = chatUserService;
        _chatRepository = chatRepository;
    }

    public async ValueTask<ChatMessage> Create(PostChatMessageRequest request, ClaimsPrincipal principal)
    {
        var user = await _chatUserService.GetOrRegister(principal);
        request.AuthorId = user.Id;

        var chat = await _chatRepository.GetChatByIdAsync(request.ChatId);
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));
        
        if (chat.ChatUsers.Contains(user) is false)
            throw new UnauthorizedException();
        
        return new ChatMessage
        {
            AuthorId = request.AuthorId,
            ChatId = request.ChatId,
            MessageText = request.Text,
        };
    }
}