using MediatR;
using Microsoft.AspNetCore.SignalR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Hubs;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.HubContracts.Chats;
using WASMChat.Shared.Requests.Chats.Messages;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Server.Handlers.Chats.Messages;

public class PostChatMessageHandler : IRequestHandler<PostChatMessageRequest, PostChatMessageResult>
{
    private readonly ChatMessageService _chatMessageService;
    private readonly ChatMessageModelMapper _chatMessageModelMapper;
    private readonly ChatUserService _chatUserService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<PostChatMessageHandler> _logger;

    public PostChatMessageHandler(
        ChatMessageService chatMessageService, 
        ChatMessageModelMapper chatMessageModelMapper,
        ChatUserService chatUserService, 
        IHubContext<ChatHub> hubContext,
        ILogger<PostChatMessageHandler> logger)
    {
        _chatMessageService = chatMessageService;
        _chatMessageModelMapper = chatMessageModelMapper;
        _chatUserService = chatUserService;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task<PostChatMessageResult> Handle(PostChatMessageRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.User, nameof(request.User));
        
        ChatUser user = await _chatUserService.GetOrRegisterAsync(request.User);
        request = request with { AuthorId = user.Id };

        ChatMessage message = await _chatMessageService.SendMessageAsync(
            request.Text, 
            request.AuthorId, 
            request.ChatId);
        message.Author = user;
        
        var result = new PostChatMessageResult
        {
            Message = _chatMessageModelMapper.Create(message)
        };

        var groupName = $"Chat_{request.ChatId}";
        
        _logger.LogInformation("Sending message {Message} to group {Group}",
            result, groupName);

        await _hubContext.Clients
            .Group(groupName)
            .SendCoreAsync(
                nameof(IChatHubClient.MessagePosted), 
                new object?[] { result }, 
                cancellationToken);
        
        _logger.LogInformation("Sent message {Message} to group {Group}",
            result, groupName);
        
        return result;
    }
}