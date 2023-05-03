using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WASMChat.Server.Models;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.EntityConfigurations;

public class ApplicationUserEntityConfiguration: IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(x => x.ChatUser)
            .WithOne()
            .HasForeignKey<ChatUser>(x => x.ApplicationUserId);
    }
}