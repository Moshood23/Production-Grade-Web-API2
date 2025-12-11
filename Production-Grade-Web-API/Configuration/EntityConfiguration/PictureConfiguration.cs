namespace Production.Grade.WebApi.Infrastructure.Configuration.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.Grade.WebApi.Domain.Entities;

public class PictureConfiguration : IEntityTypeConfiguration<Picture>
{
    public void Configure(EntityTypeBuilder<Picture> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ProductId)
            .IsRequired();

        builder.Property(p => p.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.AltText)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.AddedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(p => new { p.ProductId, p.DisplayOrder })
            .HasDatabaseName("IX_Picture_ProductId_DisplayOrder")
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(p => p.Product)
            .WithMany(prod => prod.Pictures)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}