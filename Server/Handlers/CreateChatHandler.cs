using MediatR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers;

public class CreateChatHandler : IRequestHandler<CreateChatRequest, CreateChatResult>
{
    private readonly ChatService _chatService;
    private readonly ChatModelMapper _chatModelMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ChatUserService _chatUserService;

    public CreateChatHandler(
        ChatService chatService, 
        ChatModelMapper chatModelMapper, 
        IHttpContextAccessor httpContextAccessor, 
        ChatUserService chatUserService)
    {
        _chatService = chatService;
        _chatModelMapper = chatModelMapper;
        _httpContextAccessor = httpContextAccessor;
        _chatUserService = chatUserService;
    }

    public async Task<CreateChatResult> Handle(CreateChatRequest request, CancellationToken cancellationToken)
    {
        HttpContext ctx = _httpContextAccessor.GetContext();
        
        ChatUser user = await _chatUserService.GetOrRegisterAsync(ctx.User);
        request = request with { OwnerId = user.Id };
        
        Chat chat = await _chatService.CreateChatAsync(
            request.ChatName, 
            request.OwnerId, 
            request.MemberIds);
        
        var result = new CreateChatResult
        {
            Chat = _chatModelMapper.Create(chat)
        };
        return result;
    }
}