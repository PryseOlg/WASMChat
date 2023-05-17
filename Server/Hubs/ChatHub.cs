using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Exceptions;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.HubContracts.Chats;
using WASMChat.Shared.Requests.Chats.Messages;

namespace WASMChat.Server.Hubs;

[Authorize]
public class ChatHub : Hub<IChatHubClient>, IChatHub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IMediator _mediator;
    private readonly ChatUserService _chatUserService;
    private readonly ChatService _chatService;

    public ChatHub(
        ILogger<ChatHub> logger, 
        IMediator mediator,
        ChatUserService chatUserService,
        ChatService chatService)
    {
        _logger = logger;
        _mediator = mediator;
        _chatUserService = chatUserService;
        _chatService = chatService;
    }

    public Task PostMessage(PostChatMessageRequest request)
    {
        return _mediator.Send(request with { User = Context.User });
    }

    public Task DeleteMessage(DeleteChatMessageRequest request)
    {
        return _mediator.Send(request with { User = Context.User });
    }

    public Task EditMessage(EditChatMessageRequest request)
    {
        return _mediator.Send(request with { User = Context.User });
    }

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

        var chatIds = await _chatService.GetAllChatIds(user.Id);

        foreach (var chatId in chatIds)
        {
            var groupName = $"Chat_{chatId}";
            _logger.LogInformation("Adding connection {ConnId} to group {GroupName}",
                Context.ConnectionId, groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}