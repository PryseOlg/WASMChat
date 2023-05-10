using MediatR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers;

public class GetChatHandler : IRequestHandler<GetChatRequest, GetChatResult>
{
    private readonly ChatService _chatService;
    private readonly ChatModelMapper _chatModelMapper;
    private readonly ChatUserService _chatUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetChatHandler(
        ChatService chatService, 
        ChatModelMapper chatModelMapper, 
        ChatUserService chatUserService, 
        IHttpContextAccessor httpContextAccessor)
    {
        _chatService = chatService;
        _chatModelMapper = chatModelMapper;
        _chatUserService = chatUserService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetChatResult> Handle(GetChatRequest request, CancellationToken cancellationToken)
    {
        HttpContext ctx = _httpContextAccessor.GetContext();

        ChatUser user = await _chatUserService.GetOrRegisterAsync(ctx.User);
        request = request with { UserId = user.Id };
        
        Chat chat = await _chatService.GetChatAsync(request.ChatId, request.UserId);
        
        var result = new GetChatResult
        {
            Chat = _chatModelMapper.Create(chat)
        };
        return result;
    }
}