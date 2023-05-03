﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.EntityConfigurations;

public class ChatEntityConfiguration: IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");
        
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId);
        
        builder.HasMany(x => x.ChatUsers)
            .WithMany(x => x.Chats)
            .UsingEntity(e => e.ToTable("UsersInChats"));
    }
}