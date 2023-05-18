using System.Collections.Concurrent;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Logging;
using WASMChat.CommonComponents.Extensions;
using WASMChat.Shared.Models.Chats;
using WASMChat.Shared.Results.Chats;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Pages.Chat.Storage;

public class SharedChatDataStorage
{
    private readonly HttpClient _http;
    private readonly ChatHubClient _chatHubClient;
    private readonly IDictionary<int, ChatUserModel> _users = new ConcurrentDictionary<int, ChatUserModel>();
    private readonly IDictionary<int, StoredChatData> _chats = new ConcurrentDictionary<int, StoredChatData>();
    private readonly IDictionary<int, ChatMessageModel> _messages = new ConcurrentDictionary<int, ChatMessageModel>();
    private bool _allUsersLoaded = false;
    private int? _currentUserId;

    public SharedChatDataStorage(
        HttpClient http, 
        ChatHubClient chatHubClient)
    {
        _http = http;
        _chatHubClient = chatHubClient;
        _chatHubClient.OnMessagePosted += AddMessage;
        _chatHubClient.OnMessageEdited += EditMessage;
        _chatHubClient.OnMessageDeleted += DeleteMessage;
        _chatHubClient.OnChatCreated += CreateChat;
    }

    public async Task<ChatUserModel> GetCurrentUserAsync()
    {
        if (_currentUserId is not null) 
            return _users[_currentUserId.Value];

        var result = await _http.SafeFetch<GetCurrentChatUserResult>("api/Chats/users/current");
        ChatUserModel currentUser = result.User;
        _currentUserId = currentUser.Id;
        _users[currentUser.Id] = currentUser;
        return currentUser;
    }
    public async Task<IReadOnlyCollection<ChatUserModel>> GetUsersAsync()
    {
        if (_allUsersLoaded) return _users.Values.ToArray();

        var result = await _http.SafeFetch<GetAllUsersResult>("api/Chats/users");
        var users = result.Users;
        foreach (var user in users)
        {
            _users[user.Id] = user;
        }
        _allUsersLoaded = true;
        return _users.Values.ToArray();
    }
    public async Task<IReadOnlyCollection<ChatModel>> GetChatsAsync()
    {
        if (_chats.Any()) return _chats.Values.Select(c => c.Chat).ToArray();

        var result = await _http.SafeFetch<GetAllChatsResult>("api/Chats");
        var chats = result.Chats;
        foreach (var chat in chats)
        {
            _chats.TryAdd(chat.Id, new StoredChatData { Chat = chat, IsPreviewLoaded = true });
            foreach (var msg in chat.Messages)
            {
                _messages[msg.Id] = msg;
            }
        }

        return chats;
    }
    public async Task<ChatModel> GetChatAsync(int id)
    {
        if (_chats.TryGetValue(id, out StoredChatData? savedChat) 
            && savedChat.IsPreviewLoaded is false) 
            return savedChat.Chat;
        
        var result = await _http.GetFromJsonAsync<GetChatResult>($"api/Chats/{id}");
        var chat = result!.Chat;
        _chats[id] = new StoredChatData { Chat = chat, IsPreviewLoaded = false };
        foreach (var msg in chat.Messages)
        {
            _messages[msg.Id] = msg;
        }
        
        return _chats[id].Chat;
    }

    private void AddMessage(PostChatMessageResult result)
        => _messages.Add(result.Message.Id, result.Message);

    private void EditMessage(EditChatMessageResult result)
        => _messages[result.EditedMessage.Id] = result.EditedMessage;

    private void DeleteMessage(DeleteChatMessageResult result)
        => _messages.Remove(result.MessageId);

    private void CreateChat(CreateChatResult result)
        => _chats[result.Chat.Id] = new StoredChatData { Chat = result.Chat, IsPreviewLoaded = true };
}