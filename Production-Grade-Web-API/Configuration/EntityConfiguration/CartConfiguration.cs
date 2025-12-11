namespace Production.Grade.WebApi.Infrastructure.Configuration.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.Grade.WebApi.Domain.Entities;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.LastModifiedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(c => c.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Cart_UserId");

        builder.HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CartItems)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}