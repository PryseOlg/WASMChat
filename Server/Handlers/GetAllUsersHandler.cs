using MediatR;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, GetAllUsersResult>
{
    private readonly ChatUserService _chatUserService;
    private readonly ChatUserModelMapper _chatUserModelMapper;

    public GetAllUsersHandler(
        ChatUserService chatUserService, 
        ChatUserModelMapper chatUserModelMapper)
    {
        _chatUserService = chatUserService;
        _chatUserModelMapper = chatUserModelMapper;
    }


    public async Task<GetAllUsersResult> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await _chatUserService.GetAllUsersAsync(request.Page);
        
        var result = new GetAllUsersResult()
        {
            Users = users.Select(_chatUserModelMapper.Create).ToArray()
        };
        return result;
    }
}