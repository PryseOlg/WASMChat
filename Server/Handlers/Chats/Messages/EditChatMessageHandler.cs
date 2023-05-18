using MediatR;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats.Messages;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Server.Handlers.Chats.Messages;

public class EditChatMessageHandler : IRequestHandler<EditChatMessageRequest, EditChatMessageResult>
{
    private readonly ChatMessageService _chatMessageService;
    private readonly ChatMessageModelMapper _chatMessageModelMapper;
    private readonly ChatUserService _chatUserService;

    public EditChatMessageHandler(
        ChatMessageService chatMessageService,
        ChatMessageModelMapper chatMessageModelMapper,
        ChatUserService chatUserService)
    {
        _chatMessageService = chatMessageService;
        _chatMessageModelMapper = chatMessageModelMapper;
        _chatUserService = chatUserService;
    }

    public async Task<EditChatMessageResult> Handle(EditChatMessageRequest request, CancellationToken cancellationToken)
    {
        ChatUser author = await _chatUserService.GetOrRegisterAsync(request.User!);
        
        ChatMessage msg = await _chatMessageService.EditMessageAsync(
            request.MessageId,
            author.Id,
            request.ReferencedMessageId,
            request.NewText);

        var result = new EditChatMessageResult
        {
            EditedMessage = _chatMessageModelMapper.Create(msg)
        };
        return result;
    }
}