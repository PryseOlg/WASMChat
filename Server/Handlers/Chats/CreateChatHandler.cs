using MediatR;
using Microsoft.AspNetCore.SignalR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Hubs;
using WASMChat.Server.Hubs.Utilities;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.HubContracts.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers.Chats;

public class CreateChatHandler : IRequestHandler<CreateChatRequest, CreateChatResult>
{
    private readonly ChatService _chatService;
    private readonly ChatModelMapper _chatModelMapper;
    private readonly ChatUserService _chatUserService;
    private readonly IHubContext<ChatHub, IChatHubClient> _hubContext;
    private readonly ChatHubConnectionsHelper _hubConnectionsHelper;


    public CreateChatHandler(
        ChatService chatService, 
        ChatModelMapper chatModelMapper,
        ChatUserService chatUserService, 
        IHubContext<ChatHub, IChatHubClient> hubContext,
        ChatHubConnectionsHelper hubConnectionsHelper)
    {
        _chatService = chatService;
        _chatModelMapper = chatModelMapper;
        _chatUserService = chatUserService;
        _hubContext = hubContext;
        _hubConnectionsHelper = hubConnectionsHelper;
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

        var chatGroupName = ChatHubConnectionsHelper.GetChatGroupName(result.Chat.Id);
        foreach (var member in result.Chat.Users)
        {
            _hubConnectionsHelper.AddUserToChat(result.Chat.Id, member.Id);
        }

        await _hubContext.Clients.Group(chatGroupName).ChatCreated(result);
        
        return result;
    }
}