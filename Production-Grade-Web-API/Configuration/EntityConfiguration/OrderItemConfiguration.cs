namespace Production.Grade.WebApi.Infrastructure.Configuration.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.Grade.WebApi.Domain.Entities;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.OrderId)
            .IsRequired();

        builder.Property(oi => oi.ProductId)
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.PriceAtTime)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(oi => oi.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(oi => oi.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(oi => oi.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(oi => oi.OrderId)
            .HasDatabaseName("IX_OrderItem_OrderId");

        builder.HasIndex(oi => oi.ProductId)
            .HasDatabaseName("IX_OrderItem_ProductId");

        builder.HasIndex(oi => new { oi.OrderId, oi.ProductId })
            .HasDatabaseName("IX_OrderItem_OrderId_ProductId")
            .HasFilter("[IsDeleted] = 0");

        builder.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0");
        builder.HasCheckConstraint("CK_OrderItem_PriceAtTime", "[PriceAtTime] >= 0");

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}