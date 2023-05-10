using MediatR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers;

public class GetAllChatsHandler : IRequestHandler<GetAllChatsRequest, GetAllChatsResult>
{
    private readonly ChatService _chatService;
    private readonly ChatModelMapper _chatModelMapper;
    private readonly ChatUserService _chatUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllChatsHandler(
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

    public async Task<GetAllChatsResult> Handle(GetAllChatsRequest request, CancellationToken cancellationToken)
    {
        HttpContext ctx = _httpContextAccessor.GetContext();
        
        ChatUser user = await _chatUserService.GetOrRegisterAsync(ctx.User);
        request.UserId = user.Id;
        
        var chats = await _chatService.GetAllChatsAsync(request.UserId, request.Page);
        
        var result = new GetAllChatsResult
        {
            Chats = chats.Select(_chatModelMapper.Create).ToArray()
        };
        return result;
    }
}