using MediatR;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers.Chats;

public class SetAvatarHandler : IRequestHandler<SetAvatarRequest, SetAvatarResult>
{
    private readonly ChatUserService _chatUserService;
    private readonly ChatUserModelMapper _chatUserModelMapper;

    public SetAvatarHandler(
        ChatUserService chatUserService,
        ChatUserModelMapper chatUserModelMapper)
    {
        _chatUserService = chatUserService;
        _chatUserModelMapper = chatUserModelMapper;
    }

    public async Task<SetAvatarResult> Handle(SetAvatarRequest request, CancellationToken cancellationToken)
    {
        var currentUser = await _chatUserService.GetOrRegisterAsync(request.User!);
        currentUser.AvatarId = request.FileId;

        var updatedUser = await _chatUserService.UpdateUser(currentUser);

        var result = new SetAvatarResult
        {
            UpdatedUser = _chatUserModelMapper.Create(updatedUser)
        };
        return result;
    }
}