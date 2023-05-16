using MediatR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers.Chats;

public class GetAllChatsHandler : IRequestHandler<GetAllChatsRequest, GetAllChatsResult>
{
    private readonly ChatService _chatService;
    private readonly ChatModelMapper _chatModelMapper;
    private readonly ChatUserService _chatUserService;

    public GetAllChatsHandler(
        ChatService chatService, 
        ChatModelMapper chatModelMapper, 
        ChatUserService chatUserService)
    {
        _chatService = chatService;
        _chatModelMapper = chatModelMapper;
        _chatUserService = chatUserService;
    }

    public async Task<GetAllChatsResult> Handle(GetAllChatsRequest request, CancellationToken cancellationToken)
    {
        ChatUser user = await _chatUserService.GetOrRegisterAsync(request.User);

        var chats = await _chatService.GetChatsAsync(user.Id, request.Page);
        
        var result = new GetAllChatsResult
        {
            Chats = chats.Select(_chatModelMapper.Create).ToArray()
        };
        return result;
    }
}