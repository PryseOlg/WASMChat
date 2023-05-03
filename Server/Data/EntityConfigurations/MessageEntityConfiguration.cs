using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.EntityConfigurations;

public class MessageEntityConfiguration: IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");
        
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Author)
            .WithMany(x => x.Messages);

        builder.Property(x => x.DateTimeSent)
            .ValueGeneratedOnAdd();
    }
}