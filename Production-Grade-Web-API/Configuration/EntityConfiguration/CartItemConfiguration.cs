namespace Production.Grade.WebApi.Infrastructure.Configuration.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.Grade.WebApi.Domain.Entities;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.CartId)
            .IsRequired();

        builder.Property(ci => ci.ProductId)
            .IsRequired();

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Property(ci => ci.PriceAtAddTime)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ci => ci.AddedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(ci => ci.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(ci => ci.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(ci => ci.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(ci => ci.CartId)
            .HasDatabaseName("IX_CartItem_CartId");

        builder.HasIndex(ci => ci.ProductId)
            .HasDatabaseName("IX_CartItem_ProductId");

        builder.HasIndex(ci => new { ci.CartId, ci.ProductId })
            .IsUnique()
            .HasDatabaseName("IX_CartItem_CartId_ProductId")
            .HasFilter("[IsDeleted] = 0");

        builder.HasCheckConstraint("CK_CartItem_Quantity", "[Quantity] > 0");
        builder.HasCheckConstraint("CK_CartItem_PriceAtAddTime", "[PriceAtAddTime] >= 0");

        builder.HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}