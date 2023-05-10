using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.HubClients;

public interface IChatHubClient
{
    public Task ReceiveMessage(PostChatMessageResult result);
}