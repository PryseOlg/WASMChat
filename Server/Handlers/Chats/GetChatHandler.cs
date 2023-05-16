using MediatR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers.Chats;

public class GetChatHandler : IRequestHandler<GetChatRequest, GetChatResult>
{
    private readonly ChatService _chatService;
    private readonly ChatModelMapper _chatModelMapper;
    private readonly ChatUserService _chatUserService;
    private readonly ChatUserModelMapper _chatUserModelMapper;

    public GetChatHandler(
        ChatService chatService, 
        ChatModelMapper chatModelMapper, 
        ChatUserService chatUserService,
        ChatUserModelMapper chatUserModelMapper)
    {
        _chatService = chatService;
        _chatModelMapper = chatModelMapper;
        _chatUserService = chatUserService;
        _chatUserModelMapper = chatUserModelMapper;
    }

    public async Task<GetChatResult> Handle(GetChatRequest request, CancellationToken cancellationToken)
    {
        ChatUser user = await _chatUserService.GetOrRegisterAsync(request.User!);

        Chat chat = await _chatService.GetChatAsync(request.ChatId, user.Id);
        
        var result = new GetChatResult
        {
            CurrentUser = _chatUserModelMapper.Create(user),
            Chat = _chatModelMapper.Create(chat)
        };
        return result;
    }
}