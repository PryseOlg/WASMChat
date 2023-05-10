using MediatR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers;

public class GetChatUserHandler : IRequestHandler<GetChatUserRequest, GetChatUserResult>
{
    private readonly ChatUserService _chatUserService;
    private readonly ChatUserModelMapper _chatUserModelMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetChatUserHandler(
        ChatUserService chatUserService, 
        ChatUserModelMapper chatUserModelMapper, 
        IHttpContextAccessor httpContextAccessor)
    {
        _chatUserService = chatUserService;
        _chatUserModelMapper = chatUserModelMapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetChatUserResult> Handle(GetChatUserRequest request, CancellationToken cancellationToken)
    {
        HttpContext ctx = _httpContextAccessor.GetContext();
        
        ChatUser user = await _chatUserService.GetOrRegisterAsync(ctx.User);
        
        var result = new GetChatUserResult
        {
            User = _chatUserModelMapper.Create(user)
        };
        return result;
    }
}