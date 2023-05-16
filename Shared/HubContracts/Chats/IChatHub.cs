using WASMChat.Shared.Requests.Chats.Messages;

namespace WASMChat.Shared.HubContracts.Chats;

public interface IChatHub
{
    public Task PostMessage(PostChatMessageRequest request);
    public Task DeleteMessage(DeleteChatMessageRequest request);
    public Task EditMessage(EditChatMessageRequest request);
}