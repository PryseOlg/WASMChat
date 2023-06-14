using MediatR;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers.Chats;

public class UpdateCurrentUserHandler : IRequestHandler<UpdateCurrentUserRequest, UpdateCurrentUserResult>
{
    private readonly ChatUserService _chatUserService;
    private readonly ChatUserModelMapper _chatUserModelMapper;

    public UpdateCurrentUserHandler(
        ChatUserService chatUserService,
        ChatUserModelMapper chatUserModelMapper)
    {
        _chatUserService = chatUserService;
        _chatUserModelMapper = chatUserModelMapper;
    }

    public async Task<UpdateCurrentUserResult> Handle(UpdateCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var currentUser = await _chatUserService.GetOrRegisterAsync(request.User!);
        currentUser.AvatarId = request.AvatarId;
        currentUser.Name = request.UserName;

        var updatedUser = await _chatUserService.UpdateUser(currentUser);

        var result = new UpdateCurrentUserResult
        {
            UpdatedUser = _chatUserModelMapper.Create(updatedUser)
        };
        return result;
    }
}