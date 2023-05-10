using MediatR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers;

public class PostChatMessageHandler : IRequestHandler<PostChatMessageRequest, PostChatMessageResult>
{
    private readonly ChatMessageService _chatMessageService;
    private readonly ChatMessageModelMapper _chatMessageModelMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ChatUserService _chatUserService;

    public PostChatMessageHandler(
        ChatMessageService chatMessageService, 
        ChatMessageModelMapper chatMessageModelMapper, 
        IHttpContextAccessor httpContextAccessor, 
        ChatUserService chatUserService)
    {
        _chatMessageService = chatMessageService;
        _chatMessageModelMapper = chatMessageModelMapper;
        _httpContextAccessor = httpContextAccessor;
        _chatUserService = chatUserService;
    }

    public async Task<PostChatMessageResult> Handle(PostChatMessageRequest request, CancellationToken cancellationToken)
    {
        HttpContext ctx = _httpContextAccessor.GetContext();
        
        ChatUser user = await _chatUserService.GetOrRegisterAsync(ctx.User);
        request = request with { AuthorId = user.Id };

        ChatMessage message = await _chatMessageService.SendMessageAsync(
            request.Text, 
            request.AuthorId, 
            request.ChatId);
        
        var result = new PostChatMessageResult
        {
            Message = _chatMessageModelMapper.Create(message)
        };
        return result;
    }
}