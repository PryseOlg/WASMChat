using MediatR;
using Microsoft.AspNetCore.SignalR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Hubs;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.HubClients;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers;

public class PostChatMessageHandler : IRequestHandler<PostChatMessageRequest, PostChatMessageResult>
{
    private readonly ChatMessageService _chatMessageService;
    private readonly ChatMessageModelMapper _chatMessageModelMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ChatUserService _chatUserService;
    private readonly IHubContext<ChatHub, IChatHubClient> _hubContext;

    public PostChatMessageHandler(
        ChatMessageService chatMessageService, 
        ChatMessageModelMapper chatMessageModelMapper, 
        IHttpContextAccessor httpContextAccessor, 
        ChatUserService chatUserService, 
        IHubContext<ChatHub, IChatHubClient> hubContext)
    {
        _chatMessageService = chatMessageService;
        _chatMessageModelMapper = chatMessageModelMapper;
        _httpContextAccessor = httpContextAccessor;
        _chatUserService = chatUserService;
        _hubContext = hubContext;
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

        await _hubContext.Clients
            .Group($"Chat_{request.ChatId}")
            .ReceiveMessage(result);
        return result;
    }
}