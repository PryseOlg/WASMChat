using System.Net.Http.Json;
using WASMChat.Shared.Models.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Pages.Chat.Services;

public class CurrentUserAccessor
{
    private ChatUserModel? _current;
    private readonly HttpClient _http;

    public CurrentUserAccessor(
        HttpClient http)
    {
        _http = http;
    }

    public async ValueTask<ChatUserModel> GetCurrentUserAsync()
    {
        _current ??= await FetchAsync();
        return _current;
    }

    private async ValueTask<ChatUserModel> FetchAsync()
    {
        var result = await _http.GetFromJsonAsync<GetChatUserResult>($"api/Chats/users/current");
        return result!.User;
    }
}