using WASMChat.Pages.Chat.Storage.Abstraction;
using WASMChat.Shared.Models.Chats;

namespace WASMChat.Pages.Chat.Storage.Default;

public class ChatDataProxy : IRemoteDataProxy<IReadOnlyCollection<ChatModel>>
{
    private readonly HttpClient _http;

    public ChatDataProxy(
        HttpClient http)
    {
        _http = http;
    }
    
    public Task<IReadOnlyCollection<ChatModel>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public event Action<IReadOnlyCollection<ChatModel>>? DataUpdated;
}