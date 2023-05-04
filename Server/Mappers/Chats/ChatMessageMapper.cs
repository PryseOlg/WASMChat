using System.Security.Claims;
using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories;
using WASMChat.Server.Exceptions;
using WASMChat.Server.Services;
using WASMChat.Server.Services.Chats;
using WASMChat.Server.Validators.Chats;
using WASMChat.Shared.Requests.Chats;

namespace WASMChat.Server.Mappers.Chats;

public class ChatMessageMapper : IMapper
{
    private readonly ChatUserService _chatUserService;
    private readonly ChatRepository _chatRepository;
    private readonly PostChatMessageRequestValidator _postChatMessageRequestValidator;

    public ChatMessageMapper(
        ChatUserService chatUserService, 
        ChatRepository chatRepository, 
        PostChatMessageRequestValidator postChatMessageRequestValidator)
    {
        _chatUserService = chatUserService;
        _chatRepository = chatRepository;
        _postChatMessageRequestValidator = postChatMessageRequestValidator;
    }

    public async ValueTask<ChatMessage> CreateAndValidate(PostChatMessageRequest request, ClaimsPrincipal user)
    {
        _postChatMessageRequestValidator.Validate(request);

        var chatUser = await _chatUserService.GetOrRegister(user);
        
        var chat = await _chatRepository.GetChatByIdAsync(request.ChatId);
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));
        
        if (chat.ChatUsers.Contains(chatUser) is false)
            throw new UnauthorizedException();
        
        return new ChatMessage
        {
            AuthorId = request.AuthorId,
            ChatId = request.ChatId,
            MessageText = request.Text,
        };
    }
}