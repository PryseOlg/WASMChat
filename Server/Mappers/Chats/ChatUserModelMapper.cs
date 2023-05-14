using WASMChat.CodeGenerators.Attributes;
using WASMChat.Data.Entities.Chats;
using WASMChat.Shared.Models.Chats;

namespace WASMChat.Server.Mappers.Chats;

[InjectScoped]
public class ChatUserModelMapper : IMapper
{
    public ChatUserModel Create(ChatUser user)
    {
        return new ChatUserModel
        {
            Id = user.Id,
            UserName = user.Name,
        };
    }
}