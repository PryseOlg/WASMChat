using WASMChat.Data.Entities.Chats;
using WASMChat.Shared.Models.Chats;

namespace WASMChat.Server.Mappers.Chats;

public class ChatModelMapper : IMapper
{
    private readonly ChatUserModelMapper _userModelMapper;
    private readonly ChatMessageModelMapper _chatMessageModelMapper;

    public ChatModelMapper(
        ChatUserModelMapper userModelMapper, 
        ChatMessageModelMapper chatMessageModelMapper)
    {
        _userModelMapper = userModelMapper;
        _chatMessageModelMapper = chatMessageModelMapper;
    }

    public ChatModel Create(Chat chat)
    {
        return new ChatModel
        {
            Id = chat.Id,
            Name = chat.Name,
            AvatarId = chat.AvatarId,
            Users = chat.ChatUsers.Select(_userModelMapper.Create).ToArray(),
            Messages = chat.Messages.Select(_chatMessageModelMapper.Create).ToArray(),
        };
    }
}