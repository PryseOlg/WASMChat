using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WASMChat.Data.Entities;
using WASMChat.Data.Entities.Chats;

namespace WASMChat.Data.EntityConfigurations;

public class ApplicationUserEntityConfiguration: IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(x => x.ChatUser)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey<ChatUser>(x => x.ApplicationUserId);
    }
}