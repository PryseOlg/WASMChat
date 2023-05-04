using WASMChat.Data.Entities.Chats;
using WASMChat.Data.Repositories;
using WASMChat.Shared.Requests.Chats;

namespace WASMChat.Server.Services.Chats;

public class ChatMessageService : IService
{
    private readonly ChatMessageRepository _chatMessageRepository;

    public ChatMessageService(
        ChatMessageRepository chatMessageRepository)
    {
        _chatMessageRepository = chatMessageRepository;
    }

    public async ValueTask<ChatMessage> SendMessageAsync(PostChatMessageRequest request)
    {
        ChatMessage message = new()
        {
            MessageText = request.Text,
            AuthorId = request.AuthorId,
            ChatId = request.ChatId,
        };

        await _chatMessageRepository.SaveMessage(message);
        
        return message;
    }

    public ValueTask<IReadOnlyCollection<ChatMessage>> FetchMessages(int chatId, int page = 0) =>
        _chatMessageRepository.GetMessages(chatId, page);
}