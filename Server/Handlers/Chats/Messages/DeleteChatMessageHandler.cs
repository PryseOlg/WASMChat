using MediatR;
using Microsoft.AspNetCore.SignalR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Hubs;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.HubContracts.Chats;
using WASMChat.Shared.Requests.Chats.Messages;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Server.Handlers.Chats.Messages;

public class DeleteChatMessageHandler : IRequestHandler<DeleteChatMessageRequest, DeleteChatMessageResult>
{
    private readonly ChatUserService _chatUserService;
    private readonly ChatMessageService _chatMessageService;
    private readonly ILogger<DeleteChatMessageHandler> _logger;
    private readonly IHubContext<ChatHub, IChatHubClient> _hubContext;

    public DeleteChatMessageHandler(
        ChatUserService chatUserService,
        ChatMessageService chatMessageService,
        ILogger<DeleteChatMessageHandler> logger,
        IHubContext<ChatHub, IChatHubClient> hubContext)
    {
        _chatUserService = chatUserService;
        _chatMessageService = chatMessageService;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task<DeleteChatMessageResult> Handle(DeleteChatMessageRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.User, nameof(request.User));
        
        ChatUser user = await _chatUserService.GetOrRegisterAsync(request.User);
        request = request with { AuthorId = user.Id };

        await _chatMessageService.DeleteMessageAsync(
            request.MessageId, 
            request.AuthorId);

        var result = new DeleteChatMessageResult
        {
            MessageId = request.MessageId,
        };

        var groupName = $"Chat_{request.ChatId}";
        
        _logger.LogInformation("Sending message {Message} to group {Group}",
            result, groupName);

        await _hubContext.Clients
            .Group(groupName)
            .MessageDeleted(result);
        
        _logger.LogInformation("Sent message {Message} to group {Group}",
            result, groupName);
        
        return result;
    }
}