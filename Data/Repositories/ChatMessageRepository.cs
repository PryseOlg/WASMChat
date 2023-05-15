using Microsoft.EntityFrameworkCore;
using WASMChat.Data.Entities.Chats;
namespace WASMChat.Data.Repositories;

public class ChatMessageRepository : RepositoryBase<ChatMessage>
{
    private const int MessagesPerPage = 100;

    public ChatMessageRepository(DbContext ctx) : base(ctx)
    { }

    public async ValueTask<ChatMessage> SaveMessageAsync(ChatMessage chatMessage)
    {
        chatMessage.DateTimeSent = DateTimeOffset.UtcNow;
        Set.Add(chatMessage);
        await CommitAsync();
        return chatMessage;
    }

    public async ValueTask<IReadOnlyCollection<ChatMessage>> GetMessagesAsync(int chatId, int page = 0) => await Set
        .Where(m => m.ChatId == chatId)
        .OrderByDescending(m => m.DateTimeSent)
        .Skip(page * MessagesPerPage)
        .Take(MessagesPerPage)
        .ToArrayAsync();
    
    public async ValueTask<IReadOnlyCollection<ChatMessage>> GetMessagesBeforeAsync(int chatId, DateTimeOffset radix) => await Set
        .Where(m => m.ChatId == chatId)
        .Where(m => m.DateTimeSent < radix)
        .OrderByDescending(m => m.DateTimeSent)
        .Take(MessagesPerPage)
        .ToArrayAsync();

    public ValueTask<ChatMessage?> GetMessageAsync(int messageId) => Set.FindAsync(messageId);

    public async ValueTask DeleteMessageAsync(ChatMessage message)
    {
        Set.Remove(message);
        await CommitAsync();
    }

    public async ValueTask EditMessageAsync(ChatMessage message)
    {
        Set.Update(message);
        await CommitAsync();
    }
}