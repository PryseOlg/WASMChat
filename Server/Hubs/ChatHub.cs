using Microsoft.AspNetCore.SignalR;
using WASMChat.Shared.HubClients;

namespace WASMChat.Server.Hubs;

public class ChatHub : Hub<IChatHubClient>
{
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(ILogger<ChatHub> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        var groupName = GetGroupName();
        Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        
        _logger.LogInformation("Added {ConnId} to group {GroupName}",
            Context.ConnectionId, groupName);

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var groupName = GetGroupName();
        Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        
        _logger.LogInformation("Removed {ConnId} from group {GroupName}",
            Context.ConnectionId, groupName);
        
        return base.OnDisconnectedAsync(exception);
    }

    private string GetGroupName()
        => $"Chat_{Context.GetHttpContext()?.Request.RouteValues["ChatId"] as string}";
}