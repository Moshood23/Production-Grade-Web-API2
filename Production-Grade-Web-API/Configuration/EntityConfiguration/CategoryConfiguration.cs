namespace Production.Grade.WebApi.Infrastructure.Configuration.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.Grade.WebApi.Domain.Entities;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(c => c.Id)
            .IsUnique()
            .HasDatabaseName("IX_Category_Id");

        builder.HasIndex(c => new { c.UserId, c.Name })
            .IsUnique()
            .HasDatabaseName("IX_Category_UserId_Name")
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}