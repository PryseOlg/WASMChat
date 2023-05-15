using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WASMChat.Data.Entities;

namespace WASMChat.Data.EntityConfigurations;

public class DatabaseFileEntityConfiguration : IEntityTypeConfiguration<DatabaseFile>
{
    public void Configure(EntityTypeBuilder<DatabaseFile> builder)
    {
        builder.ToTable("DatabaseFiles");
        
        builder.HasKey(x => x.Id);

        builder.HasAlternateKey(x => x.Name);
    }
}