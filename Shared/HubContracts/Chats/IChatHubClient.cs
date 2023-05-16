using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Shared.HubContracts.Chats;

public interface IChatHubClient
{
    public Task MessagePosted(PostChatMessageResult result);
    public Task MessageDeleted(DeleteChatMessageResult result);
    public Task MessageEdited(EditChatMessageResult result);
}