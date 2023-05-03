using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.EntityConfigurations;

public class ChatUserEntityConfiguration: IEntityTypeConfiguration<ChatUser>
{
    public void Configure(EntityTypeBuilder<ChatUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Chats)
            .WithMany(x => x.ChatUsers);
        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Author);
    }
}