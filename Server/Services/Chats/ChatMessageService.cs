using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories.Chats;
using WASMChat.Server.Exceptions;

namespace WASMChat.Server.Services.Chats;

public class ChatMessageService : IService
{
    private readonly ChatMessageRepository _chatMessageRepository;
    private readonly ChatUserService _chatUserService;

    public ChatMessageService(
        ChatMessageRepository chatMessageRepository, 
        ChatUserService chatUserService)
    {
        _chatMessageRepository = chatMessageRepository;
        _chatUserService = chatUserService;
    }

    public async ValueTask<ChatMessage> SendMessageAsync(string text, int authorId, int chatId)
    {
        ChatMessage message = new()
        {
            MessageText = text,
            AuthorId = authorId,
            ChatId = chatId,
        };

        await _chatMessageRepository.SaveMessageAsync(message);
        
        return message;
    }

    public ValueTask<IReadOnlyCollection<ChatMessage>> FetchMessagesAsync(int chatId, int page = 0) 
        => _chatMessageRepository.GetMessagesAsync(chatId, page);

    public ValueTask<IReadOnlyCollection<ChatMessage>> FetchMessagesBeforeAsync(int chatId, DateTimeOffset radix)
        => _chatMessageRepository.GetMessagesBeforeAsync(chatId, radix);

    public async ValueTask DeleteMessageAsync(int messageId, int authorId)
    {
        var message = await _chatMessageRepository.GetMessageAsync(messageId);
        
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        NotAllowedException.ThrowIf(message.AuthorId != authorId, "У вас нет прав удалять это сообщение");

        await _chatMessageRepository.DeleteMessageAsync(message);
    }

    public async ValueTask<ChatMessage> EditMessageAsync(
        int messageId, 
        int authorId, 
        int? referencedMessageId, 
        string? newText)
    {
        ChatMessage? msg = await _chatMessageRepository.GetMessageAsync(messageId);
        NotFoundException.ThrowIfNull(msg);
        NotAllowedException.ThrowIf(msg.AuthorId != authorId);

        if (newText is not null)
            msg.MessageText = newText;
        msg.ReferencedMessageId = referencedMessageId;
        await _chatMessageRepository.EditMessageAsync(msg);
        return msg;
    }
}