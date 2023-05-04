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

    public async ValueTask<IReadOnlyCollection<ChatMessage>> GetMessages(int chatId, int page = 0) => await Set
        .Where(m => m.ChatId == chatId)
        .Skip(page * MessagesPerPage)
        .Take(MessagesPerPage)
        .ToArrayAsync();

}