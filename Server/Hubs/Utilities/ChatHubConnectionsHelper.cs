using System.Collections.Concurrent;

namespace WASMChat.Server.Hubs.Utilities;

public class ChatHubConnectionsHelper
{
    private readonly ConcurrentDictionary<int, ConcurrentBag<string>> _usersConnections = new();
    private readonly ConcurrentDictionary<int, ConcurrentBag<int>> _usersInChats = new();

    public void SaveUserConnection(int userId, string connectionId)
        => _usersConnections.AddOrUpdate(
            userId, 
            new ConcurrentBag<string>(new[] { connectionId }),
            (_, b) =>
            {
                b.Add(connectionId);
                return b;
            });

    public IEnumerable<string> GetAllConnectionsOf(int userId)
        => _usersConnections.TryGetValue(userId, out var connections) ? 
            connections.ToArray() : 
            Array.Empty<string>();

    public void AddUserToChat(int chatId, int userId)
        => _usersInChats.AddOrUpdate(chatId,
            new ConcurrentBag<int>(new[] { userId }),
            (_, b) =>
            {
                b.Add(userId);
                return b;
            });

    public IEnumerable<int> GetAllUsersIn(int chatId)
        => _usersInChats.TryGetValue(chatId, out var users) ?
            users.ToArray() : 
            Array.Empty<int>();

    public static string GetChatGroupName(int chatId) => $"Chat_{chatId}";
    public static string GetUserGroupName(int userId) => $"User_{userId}";
}