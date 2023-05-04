using WASMChat.Shared.Requests.Chats;

namespace WASMChat.Server.Validators.Chats;

public class PostChatMessageRequestValidator : IValidator
{
    public void Validate(PostChatMessageRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.AuthorId, nameof(request.AuthorId));
        ArgumentNullException.ThrowIfNull(request.ChatId, nameof(request.ChatId));
    }
}