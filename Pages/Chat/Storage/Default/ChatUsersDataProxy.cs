using WASMChat.CommonComponents.Extensions;
using WASMChat.Pages.Chat.Storage.Abstraction;
using WASMChat.Shared.Models.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Pages.Chat.Storage.Default;

public class ChatUsersDataProxy : IRemoteDataProxy<IReadOnlyCollection<ChatUserModel>>
{
    private List<ChatUserModel> _users = new();
    private HttpClient _http;
    
    public ChatUsersDataProxy(
        HttpClient http)
    {
        _http = http;
    }
    
    public async Task<IReadOnlyCollection<ChatUserModel>> GetAsync()
    {
        if (_users.Any()) return _users;

        var data = await _http.SafeFetch<GetAllUsersResult>("api/chats/users");
        _users.AddRange(data.Users);
        DataUpdated?.Invoke(_users);

        return _users;
    }

    public event Action<IReadOnlyCollection<ChatUserModel>>? DataUpdated;
}