using MediatR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers.Chats;

public class CreateChatHandler : IRequestHandler<CreateChatRequest, CreateChatResult>
{
    private readonly ChatService _chatService;
    private readonly ChatModelMapper _chatModelMapper;
    private readonly ChatUserService _chatUserService;

    public CreateChatHandler(
        ChatService chatService, 
        ChatModelMapper chatModelMapper,
        ChatUserService chatUserService)
    {
        _chatService = chatService;
        _chatModelMapper = chatModelMapper;
        _chatUserService = chatUserService;
    }

    public async Task<CreateChatResult> Handle(CreateChatRequest request, CancellationToken cancellationToken)
    {
        ChatUser user = await _chatUserService.GetOrRegisterAsync(request.User!);

        Chat chat = await _chatService.CreateChatAsync(
            request.ChatName, 
            user.Id, 
            request.MemberIds);
        
        var result = new CreateChatResult
        {
            Chat = _chatModelMapper.Create(chat)
        };
        return result;
    }
}