using WASMChat.CommonComponents.Extensions;
using WASMChat.Pages.Chat.Storage.Abstraction;
using WASMChat.Shared.Models.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Pages.Chat.Storage.Default;

public class CurrentUserDataProxy : IRemoteDataProxy<ChatUserModel>
{
    private readonly HttpClient _http;

    public CurrentUserDataProxy(
        HttpClient http,
        IRemoteDataProxy<IReadOnlyCollection<ChatUserModel>> allUsersProxy)
    {
        _http = http;
        allUsersProxy.DataUpdated += TryUpdateCurrentUser;
    }

    private ChatUserModel? _currentUser;
    
    public async Task<ChatUserModel> GetAsync()
    {
        if (_currentUser is not null) 
            return _currentUser;

        var data = await _http.SafeFetch<GetCurrentChatUserResult>("api/chats/users/current");
        _currentUser = data.User;
        DataUpdated?.Invoke(_currentUser);
        return _currentUser;
    }

    public event Action<ChatUserModel>? DataUpdated;
    
    
    private void TryUpdateCurrentUser(IReadOnlyCollection<ChatUserModel> models)
    {
        _currentUser = models.FirstOrDefault(u => u.Id == _currentUser?.Id)
                       ?? _currentUser;
    }
}