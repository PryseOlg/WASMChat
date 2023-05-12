using MediatR;
using WASMChat.Shared.Requests.Chats.Messages;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Server.Handlers.Chats.Messages;

public class EditChatMessageHandler : IRequestHandler<EditChatMessageRequest, EditChatMessageResult>
{
    public Task<EditChatMessageResult> Handle(EditChatMessageRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}