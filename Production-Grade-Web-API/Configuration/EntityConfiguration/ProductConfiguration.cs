namespace Production.Grade.WebApi.Infrastructure.Configuration.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Domain.Enums;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Quantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.Status)
            .HasConversion<int>()
            .HasDefaultValue(ProductStatus.Active);

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(p => p.RowVersion)
            .IsRowVersion();

        builder.HasIndex(p => p.Id)
            .IsUnique()
            .HasDatabaseName("IX_Product_Id");

        builder.HasIndex(p => p.SKU)
            .IsUnique()
            .HasDatabaseName("IX_Product_SKU")
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(p => new { p.UserId, p.Status })
            .HasDatabaseName("IX_Product_UserId_Status")
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(p => p.CategoryId)
            .HasDatabaseName("IX_Product_CategoryId");

        builder.HasCheckConstraint("CK_Product_Price", "[Price] > 0");
        builder.HasCheckConstraint("CK_Product_Quantity", "[Quantity] >= 0");

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.Pictures)
            .WithOne(pic => pic.Product)
            .HasForeignKey(pic => pic.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
           .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.CartItems)
            .WithOne(ci => ci.Product)
            .HasForeignKey(ci => ci.ProductId)
           .OnDelete(DeleteBehavior.NoAction);
    }
}