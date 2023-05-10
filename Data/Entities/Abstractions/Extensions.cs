using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WASMChat.Data.Entities.Abstractions;

public static class Extensions
{
    /// <summary>
    /// Configures this <typeparamref name="TSoftDeletable"/> to be
    /// soft (reverse) deletable, which means marking type as deleted
    /// and ignoring deleted values on read instead of actually deleting it.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TSoftDeletable"></typeparam>
    /// <returns></returns>
    public static EntityTypeBuilder<TSoftDeletable> SoftDeletable<TSoftDeletable>(
        this EntityTypeBuilder<TSoftDeletable> builder)
        where TSoftDeletable : class, ISoftDeletable
    {
        builder.Property(i => i.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(i => i.DeletedTime)
            .HasDefaultValue(null)
            .IsRequired(false);

        builder.HasQueryFilter(i => i.IsDeleted == false);

        return builder;
    }
}