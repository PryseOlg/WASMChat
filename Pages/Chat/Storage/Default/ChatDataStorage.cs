using WASMChat.Pages.Chat.Storage.Abstraction;
using WASMChat.Shared.Models.Chats;
using WASMChat.Shared.Results.Chats;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Pages.Chat.Storage.Default;

public class ChatDataStorage : IChatDataStorage
{
    public ChatDataStorage(
        HttpClient http)
    {
        Users = new ChatUsersDataProxy(http);
        CurrentUser = new CurrentUserDataProxy(http, Users);
        
    }    
    
    public IRemoteDataProxy<ChatUserModel> CurrentUser { get; }
    public IRemoteDataProxy<IReadOnlyCollection<ChatModel>> Chats { get; }
    public IRemoteDataProxy<IReadOnlyCollection<ChatUserModel>> Users { get; }
    
    public event Action<PostChatMessageResult>? MessagePosted;
    public event Action<EditChatMessageResult>? MessageEdited;
    public event Action<DeleteChatMessageResult>? MessageDeleted;
    public event Action<CreateChatResult>? ChatCreated;
}