using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using WASMChat.CommonComponents.ApiClients;
using WASMChat.Shared.HubContracts.Chats;
using WASMChat.Shared.Requests.Chats.Messages;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Pages.Chat;

public class ChatHubClient : HubClientBase
{
    private readonly ILogger<ChatHubClient> _logger;

    public ChatHubClient(
        IAccessTokenProvider accessTokenProvider, 
        NavigationManager navigationManager,
        ILogger<ChatHubClient> logger) 
        : base(accessTokenProvider, navigationManager)
    {
        _logger = logger;
        SubscribeEvents();
    }
    
    protected override string HubRelativeUrl => "/api/hubs/chats";

    public event Action<PostChatMessageResult> OnMessagePosted;
    public event Action<DeleteChatMessageResult> OnMessageDeleted;
    public event Action<EditChatMessageResult> OnMessageEdited;

    public async Task PostMessage(PostChatMessageRequest request)
    {
        _logger.LogInformation("Sending message {Request}", request);
        await Connection.InvokeCoreAsync(nameof(IChatHub.PostMessage), new object?[] { request });
        _logger.LogInformation("Sent message {Request}", request);
    }

    private void SubscribeEvents()
    {
        Connection.On<PostChatMessageResult>(
            nameof(IChatHubClient.MessagePosted), 
            OnMessagePosted.Invoke);
        OnMessagePosted += Log;
        Connection.On<DeleteChatMessageResult>(
            nameof(IChatHubClient.MessageDeleted),
            OnMessageDeleted.Invoke);
        OnMessagePosted += Log;
        Connection.On<EditChatMessageResult>(
            nameof(IChatHubClient.MessageEdited), 
            OnMessageEdited.Invoke);
        OnMessagePosted += Log;
    }

    private void Log(object? result) => _logger.LogInformation(
        "Received result {Result}", result);
}