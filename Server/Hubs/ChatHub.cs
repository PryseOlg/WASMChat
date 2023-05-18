using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Exceptions;
using WASMChat.Server.Hubs.Utilities;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.HubContracts.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Requests.Chats.Messages;

namespace WASMChat.Server.Hubs;

[Authorize]
public class ChatHub : Hub<IChatHubClient>, IChatHub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IMediator _mediator;
    private readonly ChatUserService _chatUserService;
    private readonly ChatService _chatService;
    private readonly ChatHubConnectionsHelper _connectionsHelper;

    public ChatHub(
        ILogger<ChatHub> logger, 
        IMediator mediator,
        ChatUserService chatUserService,
        ChatService chatService, 
        ChatHubConnectionsHelper connectionsHelper)
    {
        _logger = logger;
        _mediator = mediator;
        _chatUserService = chatUserService;
        _chatService = chatService;
        _connectionsHelper = connectionsHelper;
    }

    public Task PostMessage(PostChatMessageRequest request) 
        => _mediator.Send(request with { User = Context.User });

    public Task DeleteMessage(DeleteChatMessageRequest request) 
        => _mediator.Send(request with { User = Context.User });

    public Task EditMessage(EditChatMessageRequest request) 
        => _mediator.Send(request with { User = Context.User });

    public Task CreateChat(CreateChatRequest request)
        => _mediator.Send(request with { User = Context.User });

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("New connection {Connection}!", Context.ConnectionId);
        await JoinGroups();
        await base.OnConnectedAsync();
    }

    private async Task JoinGroups()
    {
        NotAllowedException.ThrowIfNull(Context.User);
        ChatUser user = await _chatUserService.GetOrRegisterAsync(Context.User);

        await BindConnectionToUser(user.Id);
        
        var chats = await _chatService.GetChatsAsync(user.Id);
        foreach (var chat in chats)
        {
            await BindConnectionToChat(chat.Id, user.Id); 
        }
    }

    private async Task BindConnectionToUser(int userId)
    {
        var userGroup = ChatHubConnectionsHelper.GetUserGroupName(userId);
        
        _logger.LogInformation("Adding connection {ConnId} to group {GroupName}",
            Context.ConnectionId, userGroup);
        
        _connectionsHelper.SaveUserConnection(userId, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, userGroup);
    }

    private async Task BindConnectionToChat(int chatId, int userId)
    {
        var chatGroup = ChatHubConnectionsHelper.GetChatGroupName(chatId);
        
        _logger.LogInformation("Adding connection {ConnId} to group {GroupName}",
            Context.ConnectionId, chatGroup);
        
        _connectionsHelper.AddUserToChat(chatId, userId);
        await Groups.AddToGroupAsync(Context.ConnectionId, chatGroup);
    }
}