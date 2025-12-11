namespace Production.Grade.WebApi.Infrastructure.Configuration.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Domain.Enums;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(o => o.Status)
            .HasConversion<int>()
            .HasDefaultValue(OrderStatus.Pending);

        builder.Property(o => o.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(o => o.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(o => o.OrderDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(o => o.Notes)
            .HasMaxLength(500);

        builder.Property(o => o.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(o => o.OrderNumber)
            .IsUnique()
            .HasDatabaseName("IX_Order_OrderNumber")
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(o => new { o.UserId, o.Status })
            .HasDatabaseName("IX_Order_UserId_Status")
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(o => new { o.UserId, o.CreatedAt })
            .HasDatabaseName("IX_Order_UserId_CreatedAt")
            .HasFilter("[IsDeleted] = 0");

        builder.HasCheckConstraint("CK_Order_TotalPrice", "[TotalPrice] >= 0");

        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}