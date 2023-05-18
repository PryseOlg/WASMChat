using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using WASMChat.CommonComponents.ApiClients;
using WASMChat.Shared.HubContracts.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Requests.Chats.Messages;
using WASMChat.Shared.Results.Chats;
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

    public event Action<PostChatMessageResult> OnMessagePosted = null!;
    public event Action<DeleteChatMessageResult> OnMessageDeleted = null!;
    public event Action<EditChatMessageResult> OnMessageEdited = null!;
    public event Action<CreateChatResult> OnChatCreated = null!;

    public async Task PostMessage(PostChatMessageRequest request)
    {
        _logger.LogInformation("Sending message {Request}", request);
        await Connection.InvokeCoreAsync(nameof(IChatHub.PostMessage), new object?[] { request });
        _logger.LogInformation("Sent message {Request}", request);
    }

    public async Task DeleteMessage(DeleteChatMessageRequest request)
    {
        _logger.LogInformation("Sending message {Request}", request);
        await Connection.InvokeCoreAsync(nameof(IChatHub.DeleteMessage), new object?[] { request });
        _logger.LogInformation("Sent message {Request}", request);
    }

    public async Task EditMessage(EditChatMessageRequest request)
    {
        _logger.LogInformation("Sending message {Request}", request);
        await Connection.InvokeCoreAsync(nameof(IChatHub.EditMessage), new object?[] { request });
        _logger.LogInformation("Sent message {Request}", request);
    }

    public async Task CreateChat(CreateChatRequest request)
    {
        _logger.LogInformation("Sending message {Request}", request);
        await Connection.InvokeCoreAsync(nameof(IChatHub.CreateChat), new object?[] { request });
        _logger.LogInformation("Sent message {Request}", request);
    }

    private void SubscribeEvents()
    {
        Connection.On<PostChatMessageResult>(
            nameof(IChatHubClient.MessagePosted), 
            r => OnMessagePosted.Invoke(r));
        OnMessagePosted += Log;
        Connection.On<DeleteChatMessageResult>(
            nameof(IChatHubClient.MessageDeleted),
            r => OnMessageDeleted.Invoke(r));
        OnMessagePosted += Log;
        Connection.On<EditChatMessageResult>(
            nameof(IChatHubClient.MessageEdited), 
            r => OnMessageEdited.Invoke(r));
        OnMessagePosted += Log;
        Connection.On<CreateChatResult>(
            nameof(IChatHubClient.ChatCreated),
            r => OnChatCreated.Invoke(r));
        OnChatCreated += Log;
    }

    private void Log(object? result) => _logger.LogInformation(
        "Received result {Result}", result);
}